using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using Shinta;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;
using System.Collections.ObjectModel;

namespace FelicaToZaim.DB
{
    public class FelicaDB : ZaimDB
    {
        

        public ObservableCollection<FelicaData> readDB()
        {
            ObservableCollection<FelicaData> datas = new ObservableCollection<FelicaData>();
            using (SQLiteConnection conn = CreateDatabaseConnection())
            {
                using (DataContext context = new DataContext(conn))
                {

                    Table<FelicaData> table = context.GetTable<FelicaData>();
                    IQueryable<FelicaData> result =
                            from x in table
                            orderby x.CardId descending, x.Renban descending
                            select x;
                    foreach (FelicaData data in result)
                    {
                        datas.Add(data);
                    }
                }
            }
            return datas;
        }

        /**
         * DBにFelicaの支払い・チャージ項目があるか調べる。
         */
        public bool isFelicaDataExists(string cardId, int renban, string date)
        {
            int num = 0;

            using (SQLiteConnection conn = CreateDatabaseConnection())
            {
                using (DataContext context = new DataContext(conn))
                {
                    Table<FelicaData> table = context.GetTable<FelicaData>();
                    IQueryable<int?> result =
                            from x in table
                            where x.CardId == cardId && x.Renban == renban && x.Date == date
                            select x.Id

                            ;
                    num = result.Count();
                }
            }
            return (0 < num);
        }


        public int getMaxId()
        {
            int? max = 0;
            int ret = -1;
            List<FelicaData> datas = new List<FelicaData>();
            using (SQLiteConnection conn = CreateDatabaseConnection())
            {
                using (DataContext context = new DataContext(conn))
                {

                    Table<FelicaData> table = context.GetTable<FelicaData>();
                    IQueryable<int?> result =
                            from x in table
                            select x.Id
                            ;
                    max = result.Max();
                }
            }
            if (null != max)
            {
                ret = (int)max;
            }
            return ret;
        }


        // 金額
        [Column(Name = "price", DbType = "INT", CanBeNull = true)]
        public Int32? Price { get; set; }


        public bool saveFelicaDataToDB(List<FelicaData> datas)
        {
            using (SQLiteConnection conn = CreateDatabaseConnection())
            {
                // データ挿入
                using (DataContext context = new DataContext(conn))
                {
                    Table<FelicaData> table = context.GetTable<FelicaData>();
                    int idcount = getMaxId() + 1;
                    foreach (FelicaData data in datas)
                    {
                        data.Id = idcount;
                        table.InsertOnSubmit(data);
                        idcount++;
                    }
                    context.SubmitChanges();

                    Debug.WriteLine("DB にテーブルを作成しました。件数：" + table.Count().ToString());
                }
            }
            return true;
        }

        public bool saveFelicaDataToDBifnewDataExists(ObservableCollection<FelicaData> datas)
        {
            using (SQLiteConnection conn = CreateDatabaseConnection())
            {
                // データ挿入
                using (DataContext context = new DataContext(conn))
                {
                    Table<FelicaData> table = context.GetTable<FelicaData>();
                    int idcount = getMaxId() + 1;
                    foreach (FelicaData data in datas)
                    {
                        // DBに該当するデータがあるか検索する
                        if (!isFelicaDataExists(data.CardId, data.Renban, data.Date))
                        {
                            // 該当するデータが無い場合は追加する
                            data.Id = idcount;
                            table.InsertOnSubmit(data);
                            idcount++;
                        }
                        else
                        {
                            // 該当するデータがある場合には更新する
                            List<FelicaData> list = table.Where(
                                       elem => elem.CardId == data.CardId
                                    && elem.Renban == data.Renban
                                ).ToList<FelicaData>();
                            if (0 < list.Count())
                            {
                                list[0].iDate = data.iDate;
                                list[0].Time = data.Time;
                                list[0].IriLine = data.IriLine;
                                list[0].IriStation = data.IriStation;
                                list[0].De = data.De;
                                list[0].DeLine = data.DeLine;
                                list[0].DeStation = data.DeStation;
                                list[0].CategoryId = data.CategoryId;
                                list[0].Uploaded = data.Uploaded;

                            }
                        }
                    }
                    context.SubmitChanges();

                    Debug.WriteLine("DB にテーブルを作成しました。件数：" + table.Count().ToString());
                }
            }
            return true;
        }


        public static ObservableCollection<FelicaData> mergeData(ObservableCollection<FelicaData> data1, ObservableCollection<FelicaData> data2)
        {
            ObservableCollection<FelicaData> mergeData = new ObservableCollection<FelicaData>(); //data1
            foreach (FelicaData data2_cur in data2)
            {
                bool has_same_data = false;
                foreach (FelicaData data1_cur in data1)
                {
                    if (data1_cur.Renban == data2_cur.Renban && data1_cur.CardId == data2_cur.CardId)
                    {
                        mergeData.Remove(data1_cur);
                        data1_cur.Time = data2_cur.Time;
                        data1_cur.iDate = data2_cur.iDate;
                        data1_cur.Date = data2_cur.Date;
                        data1_cur.IriLine = data2_cur.IriLine;
                        data1_cur.IriStation = data2_cur.IriStation;
                        data1_cur.De = data2_cur.De;
                        data1_cur.DeLine = data2_cur.DeLine;
                        data1_cur.DeStation = data2_cur.DeStation;
                        mergeData.Add(data1_cur);
                        has_same_data = true;
                        break;
                    }
                }
                if (!has_same_data)
                {
                    mergeData.Add(data2_cur);
                }
            }
            return mergeData;
        }
    }
}

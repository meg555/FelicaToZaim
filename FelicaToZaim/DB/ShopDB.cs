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
    public class ShopDB: ZaimDB
    {


        public int getMaxId()
        {
            int? max = 0;
            int ret = -1;
            List<ShopData> datas = new List<ShopData>();
            using (SQLiteConnection conn = CreateDatabaseConnection())
            {
                using (DataContext context = new DataContext(conn))
                {

                    Table<ShopData> table = context.GetTable<ShopData>();
                    IQueryable<int?> result =
                            from x in table
                            select x.ShopId
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


        /**
         * DBにFelicaの支払い・チャージ項目があるか調べる。
         */
        public bool isShopaDataExists(int in_line, int in_station)
        {
            int num = 0;

            using (SQLiteConnection conn = CreateDatabaseConnection())
            {
                using (DataContext context = new DataContext(conn))
                {
                    Table<ShopData> table = context.GetTable<ShopData>();
                    IQueryable<int?> result =
                            from x in table
                            where x.IriLine == in_line && x.IriStation == in_station
                            select x.ShopId

                            ;
                    num = result.Count();
                }
            }
            return (0 < num);
        }

        public bool saveShopDataToDBifnewDataExists(ObservableCollection<ShopData> datas)
        {
            using (SQLiteConnection conn = CreateDatabaseConnection())
            {
                // データ挿入
                using (DataContext context = new DataContext(conn))
                {
                    Table<ShopData> table = context.GetTable<ShopData>();
                    int idcount = getMaxId() + 1;
                    foreach (ShopData data in datas)
                    {
                        if (null != data.IriLine && null != data.IriStation)
                        {
                            // DBに該当するデータがあるか検索する
                            if (!isShopaDataExists((int)data.IriLine, (int)data.IriStation))
                            {
                                // 該当するデータが無い場合は追加する
                                data.ShopId = idcount;
                                table.InsertOnSubmit(data);
                                idcount++;
                            }
                            else
                            {
                                // 該当するデータがある場合には更新する
                                List<ShopData> list = table.Where(
                                           elem => elem.IriLine == data.IriLine
                                        && elem.IriStation== data.IriStation
                                    ).ToList<ShopData>();
                                if (0 < list.Count())
                                {
                                    list[0].IriLine = data.IriLine;
                                    list[0].IriStation = data.IriStation;
                                    list[0].Iri = data.Iri;
                                    list[0].ShopName = data.ShopName;
                                }
                            }
                        }
                    }
                    context.SubmitChanges();

                    Debug.WriteLine("DB にテーブルを作成しました。件数：" + table.Count().ToString());
                }
            }
            return true;
        }

        public ObservableCollection<ShopData> readDB()
        {
            ObservableCollection<ShopData> datas = new ObservableCollection<ShopData>();
            using (SQLiteConnection conn = CreateDatabaseConnection())
            {
                using (DataContext context = new DataContext(conn))
                {

                    Table<ShopData> table = context.GetTable<ShopData>();
                    IQueryable<ShopData> result =
                            from x in table
                            orderby x.IriLine, x.IriStation
                            select x;
                    foreach(ShopData data in result)
                    {
                        datas.Add(data);
                    }
                }
            }
            return datas;
        }
    }
}

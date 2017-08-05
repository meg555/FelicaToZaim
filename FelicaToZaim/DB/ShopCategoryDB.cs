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
    public class ShopCategoryDB : ZaimDB
    {


        public int getMaxId()
        {
            int? max = 0;
            int ret = -1;
            List<ShopCategoryData> datas = new List<ShopCategoryData>();
            using (SQLiteConnection conn = CreateDatabaseConnection())
            {
                using (DataContext context = new DataContext(conn))
                {

                    Table<ShopCategoryData> table = context.GetTable<ShopCategoryData>();
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


        /**
         * DBにFelicaの支払い・チャージ項目があるか調べる。
         */
        public bool isShopaCategoryDataExists(int in_line, int in_station)
        {
            int num = 0;

            using (SQLiteConnection conn = CreateDatabaseConnection())
            {
                using (DataContext context = new DataContext(conn))
                {
                    Table<ShopCategoryData> table = context.GetTable<ShopCategoryData>();
                    IQueryable<int?> result =
                            from x in table
                            where x.IriLine == in_line && x.IriStation == in_station
                            select x.Id

                            ;
                    num = result.Count();
                }
            }
            return (0 < num);
        }

        public bool saveShopCategoryDataToDBifnewDataExists(ObservableCollection<ShopCategoryData> datas)
        {
            using (SQLiteConnection conn = CreateDatabaseConnection())
            {
                // データ挿入
                using (DataContext context = new DataContext(conn))
                {
                    Table<ShopCategoryData> table = context.GetTable<ShopCategoryData>();
                    int idcount = getMaxId() + 1;
                    foreach (ShopCategoryData data in datas)
                    {
                        if (null != data.IriLine && null != data.IriStation)
                        {
                            // DBに該当するデータがあるか検索する
                            if (!isShopaCategoryDataExists((int)data.IriLine, (int)data.IriStation))
                            {
                                // 該当するデータが無い場合は追加する
                                data.Id = idcount;
                                table.InsertOnSubmit(data);
                                idcount++;
                            }
                            else
                            {
                                // 該当するデータがある場合には更新する
                                List<ShopCategoryData> list = table.Where(
                                           elem => elem.IriLine == data.IriLine
                                        && elem.IriStation== data.IriStation
                                    ).ToList<ShopCategoryData>();
                                if (0 < list.Count())
                                {
                                    list[0].IriLine = data.IriLine;
                                    list[0].IriStation = data.IriStation;
                                    list[0].CategoryId = data.CategoryId;
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

        public ObservableCollection<ShopCategoryData> readDB()
        {
            ObservableCollection<ShopCategoryData> datas = new ObservableCollection<ShopCategoryData>();
            using (SQLiteConnection conn = CreateDatabaseConnection())
            {
                using (DataContext context = new DataContext(conn))
                {

                    Table<ShopCategoryData> table = context.GetTable<ShopCategoryData>();
                    IQueryable<ShopCategoryData> result =
                            from x in table
                            orderby x.IriLine, x.IriStation, x.CategoryId
                            select x;
                    foreach(ShopCategoryData data in result)
                    {
                        datas.Add(data);
                    }
                }
            }
            return datas;
        }
    }
}

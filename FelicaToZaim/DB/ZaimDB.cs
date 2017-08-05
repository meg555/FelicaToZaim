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

namespace FelicaToZaim.DB
{
    public class ZaimDB
    {

        public ZaimDB()
        {
            //CreateDBIfNotExist();
        }

        public void DeleteDB()
        {
            Debug.WriteLine("DB にテーブルを作成します...");

            try
            {
                // 既存の DB ファイルを削除
                File.Delete(DatabasePath);
            }
            catch
            {
                Debug.WriteLine("既存の DB ファイルを削除できませんでした。");
            }
            return;
        }

        // --------------------------------------------------------------------
        // データベースに接続
        // --------------------------------------------------------------------
        static protected SQLiteConnection CreateDatabaseConnection()
        {
            SQLiteConnectionStringBuilder connectionString = new SQLiteConnectionStringBuilder
            {
                DataSource = DatabasePath
            };
            SQLiteConnection conn = new SQLiteConnection(connectionString.ToString());
            return conn.OpenAndReturn();
        }

        // --------------------------------------------------------------------
        // データベースファイル名
        // --------------------------------------------------------------------
        static protected String DatabasePath
        {
            get { return Path.GetDirectoryName(Application.ExecutablePath) + "\\zaim.sqlite3"; }
        }
        

        static public void CreateDBIfNotExist(System.Type type)
        {
            try
            {
                Debug.WriteLine("DB にテーブルを作成します...");

                IEnumerable<ColumnAttribute> attrs = type.GetProperties().Select(x => Attribute.GetCustomAttribute(x, typeof(ColumnAttribute)) as ColumnAttribute);

                using (SQLiteConnection conn = CreateDatabaseConnection())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(conn))
                    {
                        // ユニーク制約
                        List<String> uniq = new List<String>();
                        ColumnAttribute primaryKeyAttr = null;

                        // Primaly Keyを探す
                        foreach (ColumnAttribute attr in attrs)
                        {
                            if(attr.IsPrimaryKey)
                            {
                                primaryKeyAttr = attr;
                                break;
                            }
                        }

                        uniq.Add(primaryKeyAttr.Name);

                        // テーブル作成
                        LinqUtils.CreateTable(cmd, type, uniq, primaryKeyAttr.Name);
                        //}
                        
                        // インデックス作成
#if false
                        List<String> index = new List<String>();
                        index.Add("test_name");
                        index.Add("test_height");
                        LinqUtils.CreateIndex(cmd, typeof(FelicaData), index);
#endif
                        Debug.WriteLine("DB にテーブルを作成しました。");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        public bool isShopDataExists(int iri_line, int iri_station)
        {
            int num = 0;

            using (SQLiteConnection conn = CreateDatabaseConnection())
            {
                using (DataContext context = new DataContext(conn))
                {
                    Table<ShopData> table = context.GetTable<ShopData>();
                    IQueryable<int?> result =
                            from x in table
                            where x.IriLine == iri_line && x.IriStation == iri_station
                            select x.ShopId

                            ;
                    num = result.Count();
                }
            }
            return (0 < num);
        }



    }


    
}

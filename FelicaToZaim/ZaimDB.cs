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

namespace FelicaToZaim
{
    public class ZaimDB
    {

        public ZaimDB()
        {
            //CreateDBIfNotExist();
            

                // ここにデータベース処理コードを書く

            
            
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
        private SQLiteConnection CreateDatabaseConnection()
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
        private String DatabasePath
        {
            get { return Path.GetDirectoryName(Application.ExecutablePath) + "\\zaim.sqlite3"; }
        }


        public void CreateDBIfNotExist()
        {
            try
            {
                Debug.WriteLine("DB にテーブルを作成します...");

                IEnumerable<ColumnAttribute> attrs = typeof(FelicaData).GetProperties().Select(x => Attribute.GetCustomAttribute(x, typeof(ColumnAttribute)) as ColumnAttribute);

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
                        //MemberInfo[] minfos = typeof(FelicaData).GetMember(nameof(FelicaData.Id));
                        //if (0 < minfos.Length)
                        //{
                        //    ColumnAttribute idAttr = (ColumnAttribute)Attribute.GetCustomAttribute(minfos[0], typeof(ColumnAttribute));
                        //    uniq.Add(idAttr.Name);
                        // テーブル作成
                        LinqUtils.CreateTable(cmd, typeof(FelicaData), uniq, primaryKeyAttr.Name);
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


        public List<FelicaData> readDB()
        {
            List < FelicaData > datas = new List<FelicaData>();
            using (SQLiteConnection conn = CreateDatabaseConnection())
            {
                using (DataContext context = new DataContext(conn))
                {

                    Table<FelicaData> table = context.GetTable<FelicaData>();
                    IQueryable<FelicaData> result =
                            from x in table
                            select x;
                    datas.AddRange(result);
                }
            }
            return datas;
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
            if(null != max)
            {
                ret = (int)max;
            }
            return ret;
        }

        public bool saveDB(List<FelicaData> datas)
        {
            using (SQLiteConnection conn = CreateDatabaseConnection())
            {
                // データ挿入
                using (DataContext context = new DataContext(conn))
                {
                    Table<FelicaData> table = context.GetTable<FelicaData>();
                    int idcount = getMaxId()+1;
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

        public static List<FelicaData> mergeData(List<FelicaData> data1, List<FelicaData> data2)
        {
            List<FelicaData> mergeData = new List<FelicaData>(data1);
            foreach (FelicaData data2_cur in data2)
            {
                bool has_same_data = false;
                foreach(FelicaData data1_cur in data1)
                {
                    if(data1_cur.Renban == data2_cur.Renban && data1_cur.CardId == data2_cur.CardId)
                    {
                        has_same_data = true;
                        break;
                    }
                }
                if(! has_same_data)
                {
                    mergeData.Add(data2_cur);
                }
            }
            return mergeData;
        }

    }


    
}

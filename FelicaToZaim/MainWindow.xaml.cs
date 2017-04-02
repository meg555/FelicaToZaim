using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FelicaLib;
using System.Data;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace FelicaToZaim
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        DataTable table;
        List<FelicaData> currentDatas = new List<FelicaData>();

        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            table = new DataTable();
            table.Columns.Add(nameof(FelicaData.Id));
            table.Columns.Add(nameof(FelicaData.CardId));
            table.Columns.Add(nameof(FelicaData.Tanmatu));
            table.Columns.Add(nameof(FelicaData.Proc));
            table.Columns.Add(nameof(FelicaData.Date));
            table.Columns.Add(nameof(FelicaData.Iri));
            table.Columns.Add(nameof(FelicaData.Zandaka));
            table.Columns.Add(nameof(FelicaData.Renban));
            table.Columns.Add(nameof(FelicaData.Price));

            this.DataContext = table;



            ZaimDB db = new ZaimDB();
#if true
            db.DeleteDB();
#endif
            db.CreateDBIfNotExist();
            List<FelicaData> felicaDatas = db.readDB();
            if (null != felicaDatas)
            {
                setFelicaDatas(felicaDatas);
            }
            return;
        }


        private void readFelicaButton_Click(object sender, RoutedEventArgs e)
        {
            Felica f = new Felica();
            List<FelicaData> felicaDatas = readSuica(f);

            List<FelicaData> mergedDatas = ZaimDB.mergeData(felicaDatas,currentDatas);
            setFelicaDatas(mergedDatas);

            ZaimDB db = new ZaimDB();
            db.saveDB(mergedDatas);
            return;
        }

        
        private List<FelicaData> readSuica(Felica f)
        {

            

            List<FelicaData> felicaDatas = new List<FelicaData>();
           


            f.Polling((int)SystemCode.Suica);

            //int cardid = FelicaUtil.suica_get_id(f);

            //int prev_zandaka = -1;

            for (int i = 0; ; i++)
            {
                FelicaData fdata = FelicaUtil.suica_dump_history(f,i);

                if (null != fdata)
                {
                    //fdata.CardId = cardid;
                    felicaDatas.Add(fdata);
                }
                else
                {
                    break;
                }
            }

            f.Dispose();

            int prev_zandaka = -1;
            for(int i=felicaDatas.Count-1; 0<=i;i--)
            {
                FelicaData data = felicaDatas[i];
                if(0 <= prev_zandaka)
                {
                    data.Price = prev_zandaka - data.Zandaka;
                }
                prev_zandaka = data.Zandaka;
            }
            return felicaDatas;
        }

        
        private void setFelicaDatas(List<FelicaData> datas)
        {
            
            currentDatas.Clear();
            table.Clear();

            foreach (FelicaData fdata in datas)
            {
                System.Data.DataRow row = table.NewRow();
                row[nameof(FelicaData.Id)] = fdata.Id;
                row[nameof(FelicaData.CardId)] = fdata.CardId;
                row[nameof(FelicaData.Tanmatu)] = fdata.Tanmatu;
                row[nameof(FelicaData.Proc   )] = fdata.Proc;
                row[nameof(FelicaData.Date   )] = fdata.Date;
                row[nameof(FelicaData.Iri    )] = fdata.Iri;
                row[nameof(FelicaData.Zandaka)] = fdata.Zandaka;
                row[nameof(FelicaData.Renban )] = fdata.Renban;
                row[nameof(FelicaData.Price)]   = fdata.Price;
                table.Rows.Add(row);
            }
            currentDatas.AddRange(datas);
            return;
        }        
    }

}

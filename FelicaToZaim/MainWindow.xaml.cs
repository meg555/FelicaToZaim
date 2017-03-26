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

namespace FelicaToZaim
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        DataTable table;

        public MainWindow()
        {

            InitializeComponent();

            table = new DataTable();
            table.Columns.Add("Tanmatu");
            table.Columns.Add("Proc");
            table.Columns.Add("Date");
            table.Columns.Add("Iri");
            table.Columns.Add("Zandaka");
            table.Columns.Add("Renban");
            table.Columns.Add("Price");

            this.DataContext = table;
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            Felica f = new Felica();
            readSuica(f);
        }

        private int prev_zandaka = -1;

        private int readSuica(Felica f)
        {
            f.Polling((int)SystemCode.Suica);

            for (int i = 0; ; i++)
            {
                FelicaData fdata = FelicaUtil.suica_dump_history(f,i);

                if (null != fdata)
                {
                    System.Data.DataRow row = table.NewRow();
                    row["Tanmatu"] = fdata.Tanmatu;
                    row["Proc"] = fdata.Proc;
                    row["Date"] = fdata.Date;
                    row["Iri"] = fdata.Iri;
                    row["Zandaka"] = fdata.Zandakav;
                    row["Renban"] = fdata.Renban;                

                    int price = 0;
                    if (0 <= prev_zandaka)
                    {
                        price = fdata.Zandakav - prev_zandaka;
                    }
                    prev_zandaka = fdata.Zandakav;
                    row["Price"] = price;
                    table.Rows.Add(row);
                }
                else
                {
                    return 0;
                }
            }

            f.Dispose();
            return 0;
        }
        
        
    }

}

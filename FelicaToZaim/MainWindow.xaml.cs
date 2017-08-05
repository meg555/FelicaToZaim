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
using System.Windows.Forms;
using FelicaLib;
using System.Data;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;

using IronRuby;
using Microsoft.Scripting.Hosting;


namespace FelicaToZaim
{
    //
    //IronRuby (Execute Ruby Script from C# Program
    //http://www.atmarkit.co.jp/fdotnet/dotnettips/1069appusingruby/appusingruby.html
    //

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window //, INotifyPropertyChanged
    {
        //DataTable table;
        ObservableCollection<FelicaData> currentFelicaDatas = new ObservableCollection<FelicaData>();
        ObservableCollection<ShopData> currentShopDatas = new ObservableCollection<ShopData>();
        ObservableCollection<ShopCategoryData> currentShopCategoryDatas = new ObservableCollection<ShopCategoryData>();

        ObservableCollection<DisplayItem> DisplayItems;

        private void RefleshData()
        {
            //dataGrid.Items.Refresh();
            setDatas();
        }


        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            DisplayItems = new ObservableCollection<DisplayItem>();

            this.dataGrid.ItemsSource = DisplayItems;
            this.DataContext = this;


#if false
            db.DeleteDB();
#endif
            DB.ZaimDB.CreateDBIfNotExist(typeof(FelicaData));
            DB.ZaimDB.CreateDBIfNotExist(typeof(ShopData));
            DB.ZaimDB.CreateDBIfNotExist(typeof(CategoryData));
            DB.ZaimDB.CreateDBIfNotExist(typeof(ShopCategoryData));

            setDatas();

            new ZaimClient();



            return;


        }

        private void setDatas()
        {
            DB.FelicaDB felicaDb = new DB.FelicaDB();
            ObservableCollection<FelicaData> felicaDatas = felicaDb.readDB();
            if (null != felicaDatas)
            {
                setFelicaDatas(felicaDatas);
            }

            DB.ShopDB shopDB = new DB.ShopDB();
            ObservableCollection<ShopData> shopDatas = shopDB.readDB();
            if (null != shopDatas)
            {
                setShopDatas(shopDatas);
            }

            DB.ShopCategoryDB shopCategoryDB = new DB.ShopCategoryDB();
            ObservableCollection<ShopCategoryData> shopCategoryDatas = shopCategoryDB.readDB();
            if (null != shopCategoryDatas)
            {
                setShopCategoryDatas(shopCategoryDatas);
            }
        }

        private void sortData(List<FelicaData> data)
        {
            data.Sort(FelicaData.Compare);
        }


        private void readFelicaButton_Click(object sender, RoutedEventArgs e)
        {
            Felica f = new Felica();
            ObservableCollection<FelicaData> felicaDatas = readSuica(f);

            ObservableCollection<FelicaData> mergedDatas = DB.FelicaDB.mergeData(currentFelicaDatas, felicaDatas);
            setFelicaDatas(mergedDatas);



            DB.FelicaDB felicaDb = new DB.FelicaDB();
            felicaDb.saveFelicaDataToDBifnewDataExists(mergedDatas);
            return;
        }

        
        private ObservableCollection<FelicaData> readSuica(Felica f)
        {
            ObservableCollection<FelicaData> felicaDatas = new ObservableCollection<FelicaData>();
           

            f.Polling((int)SystemCode.Suica);
            

            for (int i = 0; ; i++)
            {
                FelicaData fdata = FelicaUtil.suica_dump_history(f,i);

                if (null != fdata)
                {
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

        
        private void setFelicaDatas(ObservableCollection<FelicaData> datas)
        {
            
            currentFelicaDatas.Clear();
            //table.Clear();

            foreach (FelicaData fdata in datas)
            {
                DisplayItem item = new DisplayItem();
                item.FeliaData = (fdata);
                DisplayItems.Add(item);
            }
            foreach(FelicaData data in datas)
            {
                currentFelicaDatas.Add(data);
            }
            return;
        }

        private void updateShopDatas(ObservableCollection<ShopData> datas)
        {
            // 支払い履歴の1列ごとに処理する
            foreach (DisplayItem item in DisplayItems)
            {
                ShopData shopdata = null;
                // お店のコード番号を取得する
                int? iri_line = item.FeliaData.IriLine;
                int? iri_station = item.FeliaData.IriStation;
                if(null != iri_line &&  null != iri_station)
                {
                    // お店のコード番号に該当するお店のデータを取得する
                    ShopData.find(datas, (int)iri_line, (int)iri_station, out shopdata);
                    if (null != shopdata)
                    {
                        // お店の情報があったら設定する
                        item.ShopData = shopdata;
                    }
                }
            }
            return;
        }

        private void setShopDatas(ObservableCollection<ShopData> datas)
        {

            currentShopDatas.Clear();
            updateShopDatas(datas);
            foreach(ShopData data in datas)
            {
                currentShopDatas.Add(data);
            }

            return;
        }

        private void setShopCategoryDatas(ObservableCollection<ShopCategoryData> datas)
        {

            currentShopCategoryDatas.Clear();
            updateShopCaterogyDatas(datas);
            foreach (ShopCategoryData data in datas)
            {
                currentShopCategoryDatas.Add(data);
            }

            return;
        }

        private void updateShopCaterogyDatas(ObservableCollection<ShopCategoryData> datas)
        {
            // 支払い履歴の1列ごとに処理する
            foreach (DisplayItem item in DisplayItems)
            {
                ShopCategoryData shopcategorydata = null;
                // お店のコード番号を取得する
                int? iri_line = item.FeliaData.IriLine;
                int? iri_station = item.FeliaData.IriStation;
                int? categoryid = item.FeliaData.CategoryId;
                if (null != iri_line && null != iri_station && null == categoryid)
                {
                    // お店のコード番号に該当するお店のデータを取得する
                    ShopCategoryData.find(datas, (int)iri_line, (int)iri_station, out shopcategorydata);
                    if (null != shopcategorydata)
                    {
                        // お店の情報があったら設定する
                        item.CategoryId = shopcategorydata.CategoryId;
                    }
                }
            }
            return;
        }

        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            // プロパティ名をもとに自動生成する列をカスタマイズします
            switch (e.PropertyName)
            {
                case nameof(DisplayItem.IsCommitToServer):
                    // Name列は最初に表示してヘッダーを名前にする
                    e.Column.Header = "送信";
                    e.Column.DisplayIndex = 1;
                    break;
                case nameof(DisplayItem.Uploaded):
                    e.Column.Header = "済";
                    e.Column.DisplayIndex = 0;
                    e.Column.IsReadOnly = true;
                    break;
                case nameof(DisplayItem.category):
                    e.Column.Header = "カテゴリ";
                    e.Column.DisplayIndex = 2;
                    break;

                case nameof(DisplayItem.CategoryName):
                    e.Column.Header = "カテゴリ名";
                    e.Column.DisplayIndex = 3;
                    e.Column.IsReadOnly = true;
                    break;

                case nameof(DisplayItem.ShopName):
                    e.Column.Header = "店名";
                    e.Column.DisplayIndex = 4;
                    break;

                case nameof(DisplayItem.Iri):
                    e.Cancel = true;
                    break;
                case nameof(DisplayItem.Id):
                    e.Cancel = true;
                    break;
                case nameof(DisplayItem.CardId):
                    e.Cancel = true;
                    break;
                case nameof(DisplayItem.De):
                    e.Cancel = true;
                    break;
                case nameof(DisplayItem.Renban):
                    e.Cancel = true;
                    break;
                case nameof(DisplayItem.FeliaData):
                    e.Cancel = true;
                    break;
                case nameof(DisplayItem.CategoryData):
                    e.Cancel = true;
                    break;
                case nameof(DisplayItem.ShopData):
                    e.Cancel = true;
                    break;

                case nameof(DisplayItem.ShopId):
                    e.Cancel = true;
                    break;
                case nameof(DisplayItem.CategoryId):
                    e.Cancel = true;
                    break;

                case nameof(DisplayItem.IriLine):
                    e.Cancel = true;
                    e.Column.IsReadOnly = true;
                    break;
                case nameof(DisplayItem.IriStation):
                    e.Cancel = true;
                    e.Column.IsReadOnly = true;
                    break;

                default:
                    break;
                    //throw new InvalidOperationException();
            }
        }


        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }

        const int index_shop_name = 4;
        const int index_category_name = 2;
        //const int index_iri_line = 8;
        //const int index_iri_station = 9;


        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            //dataGrid.UnselectAll();
            switch (e.Column.DisplayIndex)
            {
                case index_shop_name:    // Shop name
                    {
                        //e.EditingElement;
                        string text = ((System.Windows.Controls.TextBox)e.EditingElement).Text;
                        DisplayItem item = (DisplayItem)e.Row.Item;


                        // create or update Shop data
                        int iri_line = (int)item.FeliaData.IriLine; // int.Parse((string)((DataRowView)row.Item).Row.ItemArray[index_iri_line]);
                        int iri_station = (int)item.FeliaData.IriStation; //int.Parse((string)((DataRowView)row.Item).Row.ItemArray[index_iri_station]);

                        ShopData shopdata = null;
                        ShopData.find(currentShopDatas, iri_line, iri_station, out shopdata);
                        bool is_created_data = false;
                        if (null == shopdata)
                        {
                            shopdata = new ShopData();
                            is_created_data = true;
                        }
                        shopdata.IriLine = iri_line;
                        shopdata.IriStation = iri_station;
                        shopdata.Iri = FelicaData.toLineShopString(iri_line, iri_station);
                        shopdata.ShopName = text;

                        if (is_created_data)
                        {
                            currentShopDatas.Add(shopdata);
                        }
                        // update display items
                        updateShopDatas(currentShopDatas);
                    }
                    break;

                case index_category_name:
                    {
                        Category category = (Category)(((System.Windows.Controls.ComboBox)e.EditingElement).SelectedItem);
                        
                        DisplayItem item = (DisplayItem)e.Row.Item;


                        ShopCategoryData shopcategorydata = null;
                        int iri_line = (int)item.IriLine;
                        int iri_station = (int)item.IriStation;
                        ShopCategoryData.find(currentShopCategoryDatas, iri_line, iri_station, out shopcategorydata);
                        bool is_created_data = false;
                        if (null == shopcategorydata)
                        {
                            shopcategorydata = new ShopCategoryData();
                            is_created_data = true;
                        }
                        shopcategorydata.IriLine = item.IriLine;
                        shopcategorydata.IriStation = item.IriStation;
                        int categoryid = category.ToId();
                        shopcategorydata.CategoryId = categoryid;

                        if (is_created_data)
                        {
                            currentShopCategoryDatas.Add(shopcategorydata);
                        }
                        updateShopCaterogyDatas(currentShopCategoryDatas);

                        // update display items
                        foreach (DisplayItem ditem in DisplayItems)
                        {
                            if (ditem.CardId == item.CardId && ditem.Renban == item.Renban)
                            {
                                ditem.category = category;
                            }
                        }
                    }
                    break;
            }

            DB.ShopDB shooDB = new DB.ShopDB();
            shooDB.saveShopDataToDBifnewDataExists(currentShopDatas);

            DB.ShopCategoryDB shooCategoryDB = new DB.ShopCategoryDB();
            shooCategoryDB.saveShopCategoryDataToDBifnewDataExists(currentShopCategoryDatas);

            return;

        }

        private void RefleshButton_Click(object sender, RoutedEventArgs e)
        {
            RefleshData();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DB.FelicaDB felicaDB= new DB.FelicaDB();
            DB.ShopDB shopdb = new DB.ShopDB();
            felicaDB.saveFelicaDataToDBifnewDataExists(currentFelicaDatas);
            shopdb.saveShopDataToDBifnewDataExists(currentShopDatas);
            RefleshData();
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            foreach(DisplayItem item in DisplayItems)
            {
                if(item.IsCommitToServer)
                {
                    ZaimClient.make_payment(item);
                    item.FeliaData.Uploaded = true;
                }
            }
            DB.FelicaDB felicaDB = new DB.FelicaDB();
            felicaDB.saveFelicaDataToDBifnewDataExists(currentFelicaDatas);
            ClearAllUploadCheck();
            RefleshData();

        }

        private void ClearAllUploadCheck()
        {
            foreach(DisplayItem item in DisplayItems)
            {
                item.IsCommitToServer = false;
            }
        }
    }

}

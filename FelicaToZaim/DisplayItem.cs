using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FelicaToZaim
{
    class DisplayItem: INotifyPropertyChanged
    {

        public bool IsCommitToServer { get; set; } = false;

        public bool Uploaded { get { return (bool)((null == FeliaData.Uploaded)?false: FeliaData.Uploaded); } set {FeliaData.Uploaded = value; } }


        //// ID
        public Int32? Id { get { return FeliaData.Id; } set { FeliaData.Id = value; } }

        // カードID
        public String CardId { get { return FeliaData.CardId; } set { FeliaData.CardId = value; } }

        // 端末
        public String Tanmatu { get { return FeliaData.Tanmatu; } set { FeliaData.Tanmatu = value; } }

        // 処理
        public String Proc { get { return FeliaData.Proc; } set { FeliaData.Proc = value; } }

        // 日付
        public String Date { get { return FeliaData.Date; } set { FeliaData.Date = value; } }
        
        // 入り
        public String Iri { get { return FeliaData.Iri; } set { FeliaData.Iri = value; } }

        // 入り
        public Int32? IriLine { get { return FeliaData.IriLine; } set { FeliaData.IriLine = value; } }

        // 入り
        public Int32? IriStation { get { return FeliaData.IriStation; } set { FeliaData.IriStation = value; } }

        // 出
        public String De { get { return FeliaData.De; } set { FeliaData.De = value; } }

        // 連番
        public Int32 Renban { get { return FeliaData.Renban; } set { FeliaData.Renban = value; } }

        // 金額
        public Int32? Price { get { return FeliaData.Price; } set { FeliaData.Price = value; } }

        // ID
        public Int32? ShopId { get { return (null== ShopData)?null:ShopData.ShopId; } set { ShopData.ShopId = value; } }

        // 店名
        public String ShopName {
            get { return ShopData.ShopName; }
            set {
                ShopData.ShopName = value;
                OnPropertyChanged(nameof(ShopName));
        } }

        // ID
        public Int32? CategoryId { get { return FeliaData.CategoryId; } set { FeliaData.CategoryId = value; } }

        // カテゴリ名
        public String CategoryName { get { return CategoryData.CategoryName; } set { CategoryData.CategoryName = value; } }

        public Category category {
            get { return CategoryData.category; }
            set {
                CategoryData = new CategoryData(value);
                FeliaData.CategoryId = value.ToId();
            }
        }

        private FelicaData _felicaData = new FelicaData();

        public FelicaData FeliaData
        {
            get { return _felicaData; }
            set
            {
                _felicaData = value;
                if (null != _felicaData.CategoryId)
                {
                    CategoryData = new CategoryData(CategoryExtensions.toCategory((int)_felicaData.CategoryId));
                }
            }
        }

        public ShopData ShopData { get; set; } = new ShopData();

        public CategoryData CategoryData { get; set; } = new CategoryData(Category.None);

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }



    }
}

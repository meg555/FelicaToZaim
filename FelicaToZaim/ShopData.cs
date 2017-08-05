using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Linq.Mapping;
using System.Collections.ObjectModel;


namespace FelicaToZaim
{
    [Table(Name = "shopdata")]
    public class ShopData
    {

        // ID
        [Column(Name = "id", DbType = "INT", CanBeNull = false, IsPrimaryKey = true)]
        public Int32? ShopId { get; set; }
        

        // 入り
        [Column(Name = "iri", DbType = "NVARCHAR", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public String Iri { get; set; }


        [Column(Name = "iriline", DbType = "INT", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public int? IriLine { get; set; }

        [Column(Name = "iristation", DbType = "INT", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public int? IriStation { get; set; }

        // 店名
        [Column(Name = "ShopName", DbType = "NVARCHAR", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public String ShopName { get; set; }


        public static void find(ObservableCollection<ShopData> list,  int iri_line, int iri_station, out ShopData shopdata)
        {
            shopdata = null;
            foreach (ShopData tmpshopdata in list)
            {
                if (iri_line == tmpshopdata.IriLine && iri_station == tmpshopdata.IriStation)
                {
                    shopdata =  tmpshopdata;
                }
            }
            return;
        }


    }

}

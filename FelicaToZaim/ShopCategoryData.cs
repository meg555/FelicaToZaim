using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Linq.Mapping;
using System.Collections.ObjectModel;


namespace FelicaToZaim
{
    [Table(Name = "shopcategorydata")]
    public class ShopCategoryData
    {

        // ID
        [Column(Name = "id", DbType = "INT", CanBeNull = false, IsPrimaryKey = true)]
        public Int32? Id { get; set; }


        [Column(Name = "iriline", DbType = "INT", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public int? IriLine { get; set; }

        [Column(Name = "iristation", DbType = "INT", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public int? IriStation { get; set; }


        // ID
        [Column(Name = "categoryid", DbType = "INT", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public Int32? CategoryId { get; set; } //{ get { return category.ToId(); } set { category = CategoryExtensions.toCategory((int)value); } }

        // 入り
        //[Column(Name = "iri", DbType = "NVARCHAR", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        //public String Iri { get; set; }

        //public Category category { get; set; }

        public static void find(ObservableCollection<ShopCategoryData> list,  int iri_line, int iri_station, out ShopCategoryData shopcategorydata)
        {
            shopcategorydata = null;
            foreach (ShopCategoryData tmpshopcategorydata in list)
            {
                if (iri_line == tmpshopcategorydata.IriLine && iri_station == tmpshopcategorydata.IriStation)
                {
                    shopcategorydata = tmpshopcategorydata;
                }
            }
            return;
        }


    }

}

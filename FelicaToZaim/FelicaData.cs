using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Linq.Mapping;


namespace FelicaToZaim
{
    [Table(Name = "felica")]
    public class FelicaData
    {


        // ID
        [Column(Name = "id", DbType = "INT", CanBeNull = false, IsPrimaryKey = true)]
        public Int32? Id { get; set; }

        // ID
        [Column(Name = "uploaded", DbType = "BIT", CanBeNull = true, UpdateCheck = UpdateCheck.Never)]
        public Boolean? Uploaded { get; set; }


        // カードID
        [Column(Name = "cardid", DbType = "NVARCHAR", CanBeNull = false)]
        public String CardId { get; set; }


        // 端末
        [Column(Name = "tanmatu", DbType = "NVARCHAR", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public String Tanmatu { get; set; }

        // 処理
        [Column(Name = "proc", DbType = "NVARCHAR", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public String Proc { get; set; }

        // 日付
        [Column(Name = "date", DbType = "NVARCHAR", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public String Date { get; set; }
        [Column(Name = "idate", DbType = "INT", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public Int32? iDate { get; set; }

        // 時刻
        [Column(Name = "time", DbType = "INT", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public Int32? Time { get; set; }

        // 入り
        [Column(Name = "iri", DbType = "NVARCHAR", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public String Iri { get; set; }

        [Column(Name = "iriline", DbType = "INT", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public int? IriLine { get; set; }

        [Column(Name = "iristation", DbType = "INT", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public int? IriStation { get; set; }

        // 出
        [Column(Name = "de", DbType = "NVARCHAR", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public String De { get; set; }

        [Column(Name = "deline", DbType = "INT", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public int? DeLine { get; set; }

        [Column(Name = "destation", DbType = "INT", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public int? DeStation { get; set; }


        // 残高
        [Column(Name = "zandaka", DbType = "INT", CanBeNull = false)]
        public Int32 Zandaka { get; set; }

        // 連番
        [Column(Name = "renban", DbType = "INT", CanBeNull = false)]
        public Int32 Renban { get; set; }

        // 金額
        [Column(Name = "price", DbType = "INT", CanBeNull = true)]
        public Int32? Price { get; set; }

        [Column(Name = "categoryid", DbType = "INT", CanBeNull = true)]
        public int? CategoryId { get; set; }


        public static int Compare(FelicaData d1, FelicaData d2)
        {
            int ret = 0;
            {
                if (d1.CardId == d2.CardId)
                {
                    ret = d1.Renban - d2.Renban;
                }
                else
                {
                    ret = d1.CardId.CompareTo(d2.CardId);
                }
                
            }
            return ret;
        }
        
        public static string toLineShopString(int in_line, int in_sta)
        {
            return in_line.ToString("X") + "/" + in_sta.ToString("X");
        }

        public static string toDateString(int yy, int mm, int dd)
        {
            return yy.ToString("00") + "/" + mm.ToString("00") + "/" + dd.ToString("00") + " ";
        }

        public static string toTimeString(int hh, int min)
        {
            return hh.ToString("00") + ":" + min.ToString("00");
        }
    }
    
}

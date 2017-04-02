using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Linq.Mapping;


namespace FelicaToZaim
{
    [Table(Name = "zaim")]
    public class FelicaData
    {

        // ID
        [Column(Name = "id", DbType = "INT", CanBeNull = false, IsPrimaryKey = true)]
        public Int32? Id { get; set; }

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

        // 入り
        [Column(Name = "iri", DbType = "NVARCHAR", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public String Iri { get; set; }

        // 残高
        [Column(Name = "zandaka", DbType = "INT", CanBeNull = false)]
        public Int32 Zandaka { get; set; }

        // 連番
        [Column(Name = "renban", DbType = "INT", CanBeNull = false)]
        public Int32 Renban { get; set; }

        // 金額
        [Column(Name = "price", DbType = "INT", CanBeNull = true)]
        public Int32? Price { get; set; }
    }
    
}

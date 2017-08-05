using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Linq.Mapping;


namespace FelicaToZaim
{
    [Table(Name = "categorydata")]
    public class CategoryData
    {

        // ID
        [Column(Name = "id", DbType = "INT", CanBeNull = false, IsPrimaryKey = true)]
        public Int32? Id { get { return category.ToId(); } set { CategoryExtensions.toCategory((int)value); } }

        public Category category { get; set; }

        // カテゴリ名
        [Column(Name = "categoryname", DbType = "NVARCHAR", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public String CategoryName { get { return category.ToLable(); } set{ CategoryExtensions.toCategory(value); } }

        

        public CategoryData(Category category)
        {
            this.category = category;
        }



    }

    

    public enum Category
    {
        [Id(0), Label("不明"), Code("0"), CategoryId(0)]
        None,
        [Id(10101),Label("食料品"),Code("0101"),CategoryId(101)]
        Foods,
        [Id(10102), Label("カフェ"), Code("0102"), CategoryId(101)]
        Cafe,
        [Id(10103), Label("朝ご飯"), Code("0103"), CategoryId(101)]
        Breakfast,
        [Id(10104), Label("昼ご飯"), Code("0104"), CategoryId(101)]
        Lunch,
        [Id(10105), Label("晩ご飯"), Code("0105"), CategoryId(101)]
        Dinner,
        [Id(10199), Label("その他"), Code("0106"), CategoryId(101)]
        OtherFoods,
        [Id(10201), Label("消耗品"), Code("0201"), CategoryId(102)]
        Commodities,

        [Id(10202), Label("子ども関連"), Code("0202"), CategoryId(102)]
        Child_Related_Materials,
        [Id(10203), Label("ペット関連"), Code("0203"), CategoryId(102)]
        Pet_Related_Materials,
        //[Id(10204), Label("タバコ"), Code("0204"), CategoryId(102)]
        //[Id(10299), Label("その他"), Code("0205"), CategoryId(102)]
        [Id(10301), Label("電車"), Code("0301"), CategoryId(103)]
        Fare_Train,
        //[Id(10302), Label("タクシー"), Code("0302"), CategoryId(103)]
        [Id(10303), Label("バス"), Code("0303"), CategoryId(103)]
        Fare_Bus,
        //[Id(10304), Label("飛行機"), Code("0304"), CategoryId(103)]
        //[Id(10399), Label("その他"), Code("0305"), CategoryId(103)]
        [Id(10701), Label("飲み会"), Code("0401"), CategoryId(107)]
        DrinkingParty,
        [Id(10702), Label("プレゼント"), Code("0402"), CategoryId(107)]
        Present,
        //[Id(10703), Label("ご祝儀・香典"), Code("0403"), CategoryId(107)]
        //[Id(10799), Label("その他"), Code("0404"), CategoryId(107)]
        //[Id(10801), Label("レジャー"), Code("0501"), CategoryId(108)]
        //[Id(10802), Label("イベント"), Code("0502"), CategoryId(108)]
        //[Id(10803), Label("映画・動画"), Code("0503"), CategoryId(108)]
        //[Id(10804), Label("音楽"), Code("0504"), CategoryId(108)]
        //[Id(10805), Label("漫画"), Code("0505"), CategoryId(108)]
        [Id(10806), Label("書籍"), Code("0506"), CategoryId(108)]
        Books,
        //[Id(10807), Label("ゲーム"), Code("0507"), CategoryId(108)]
        //[Id(10899), Label("その他"), Code("0508"), CategoryId(108)]
        //[Id(10901), Label("習い事"), Code("0601"), CategoryId(109)]
        //[Id(10902), Label("新聞"), Code("0602"), CategoryId(109)]
        //[Id(10903), Label("参考書"), Code("0603"), CategoryId(109)]
        //[Id(10904), Label("受験料"), Code("0604"), CategoryId(109)]
        //[Id(10905), Label("学費"), Code("0605"), CategoryId(109)]
        //[Id(10906), Label("学資保険"), Code("0606"), CategoryId(109)]
        //[Id(10907), Label("塾"), Code("0607"), CategoryId(109)]
        //[Id(10999), Label("その他"), Code("0608"), CategoryId(109)]
        //[Id(11101), Label("洋服"), Code("0701"), CategoryId(111)]
        //[Id(11102), Label("アクセサリー・小物"), Code("0702"), CategoryId(111)]
        //[Id(11103), Label("下着"), Code("0703"), CategoryId(111)]
        //[Id(11104), Label("ジム・健康"), Code("0704"), CategoryId(111)]
        //[Id(11105), Label("美容院"), Code("0705"), CategoryId(111)]
        //[Id(11106), Label("コスメ"), Code("0706"), CategoryId(111)]
        //[Id(11107), Label("エステ・ネイル"), Code("0707"), CategoryId(111)]
        //[Id(11108), Label("クリーニング"), Code("0708"), CategoryId(111)]
        //[Id(11199), Label("その他"), Code("0709"), CategoryId(111)]
        //[Id(11001), Label("病院代"), Code("0801"), CategoryId(110)]
        //[Id(11002), Label("薬代"), Code("0802"), CategoryId(110)]
        //[Id(11003), Label("生命保険"), Code("0803"), CategoryId(110)]
        //[Id(11004), Label("医療保険"), Code("0804"), CategoryId(110)]
        //[Id(11099), Label("その他"), Code("0805"), CategoryId(110)]
        //[Id(10401), Label("携帯電話料金"), Code("0901"), CategoryId(104)]
        //[Id(10402), Label("固定電話料金"), Code("0902"), CategoryId(104)]
        //[Id(10403), Label("インターネット関連費"), Code("0903"), CategoryId(104)]
        //[Id(10404), Label("放送サービス料金"), Code("0904"), CategoryId(104)]
        //[Id(10405), Label("宅配便"), Code("0905"), CategoryId(104)]
        //[Id(10406), Label("切手・はがき"), Code("0906"), CategoryId(104)]
        //[Id(10499), Label("その他"), Code("0907"), CategoryId(104)]
        //[Id(10501), Label("水道料金"), Code("1001"), CategoryId(105)]
        //[Id(10502), Label("電気料金"), Code("1002"), CategoryId(105)]
        //[Id(10503), Label("ガス料金"), Code("1003"), CategoryId(105)]
        //[Id(10599), Label("その他"), Code("1004"), CategoryId(105)]
        //[Id(10601), Label("家賃"), Code("1101"), CategoryId(106)]
        //[Id(10602), Label("住宅ローン返済"), Code("1102"), CategoryId(106)]
        //[Id(10603), Label("家具"), Code("1103"), CategoryId(106)]
        //[Id(10604), Label("家電"), Code("1104"), CategoryId(106)]
        //[Id(10605), Label("リフォーム"), Code("1105"), CategoryId(106)]
        //[Id(10606), Label("住宅保険"), Code("1106"), CategoryId(106)]
        //[Id(10699), Label("その他"), Code("1107"), CategoryId(106)]
        //[Id(11201), Label("ガソリン"), Code("1201"), CategoryId(112)]
        //[Id(11202), Label("駐車場"), Code("1202"), CategoryId(112)]
        //[Id(11203), Label("自動車保険"), Code("1203"), CategoryId(112)]
        //[Id(11204), Label("自動車税"), Code("1204"), CategoryId(112)]
        //[Id(11205), Label("自動車ローン"), Code("1205"), CategoryId(112)]
        //[Id(11206), Label("免許教習"), Code("1206"), CategoryId(112)]
        //[Id(11207), Label("高速料金"), Code("1207"), CategoryId(112)]
        //[Id(11299), Label("その他"), Code("1208"), CategoryId(112)]
        //[Id(11301), Label("年金"), Code("1301"), CategoryId(113)]
        //[Id(11302), Label("所得税"), Code("1302"), CategoryId(113)]
        //[Id(11303), Label("消費税"), Code("1303"), CategoryId(113)]
        //[Id(11304), Label("住民税"), Code("1304"), CategoryId(113)]
        //[Id(11305), Label("個人事業税"), Code("1305"), CategoryId(113)]
        //[Id(11399), Label("その他"), Code("1306"), CategoryId(113)]
        [Id(12667688), Label("出張立替"), Code("1401"), CategoryId(114)]
        AdvancePayment,
        //[Id(11401), Label("旅行"), Code("1402"), CategoryId(114)]
        //[Id(11402), Label("住宅"), Code("1403"), CategoryId(114)]
        //[Id(11403), Label("自動車"), Code("1404"), CategoryId(114)]
        //[Id(11404), Label("バイク"), Code("1405"), CategoryId(114)]
        //[Id(11405), Label("結婚"), Code("1406"), CategoryId(114)]
        //[Id(11406), Label("出産"), Code("1407"), CategoryId(114)]
        //[Id(11407), Label("介護"), Code("1408"), CategoryId(114)]
        //[Id(11408), Label("家具"), Code("1409"), CategoryId(114)]
        //[Id(11409), Label("家電"), Code("1410"), CategoryId(114)]
        //[Id(11499), Label("その他"), Code("1411"), CategoryId(114)]
        //[Id(19901), Label("仕送り"), Code("1501"), CategoryId(199)]
        //[Id(19902), Label("お小遣い"), Code("1502"), CategoryId(199)]
        //[Id(19903), Label("使途不明金"), Code("1503"), CategoryId(199)]
        //[Id(19904), Label("立替金"), Code("1504"), CategoryId(199)]
        //[Id(19905), Label("未分類"), Code("1505"), CategoryId(199)]
        //[Id(19906), Label("現金の引出"), Code("1506"), CategoryId(199)]
        //[Id(19999), Label("その他"), Code("1507"), CategoryId(199)]
        //[Id(19907), Label("カードの引落"), Code("1508"), CategoryId(199)]
    }

    /// <summary>
    /// from http://neue.cc/2010/11/13_285.html
    /// </summary>
    public static class CategoryExtensions
    {
        private static Dictionary<Category, IdAttribute> idCache;
        private static Dictionary<Category, LabelAttribute> labelCache;
        private static Dictionary<Category, CodeAttribute> codeCache;
        private static Dictionary<Category, CategoryIdAttribute> categoryIdCache;

        static CategoryExtensions()
        {
            // Enumから属性と値を取り出す。
            // この部分は汎用的に使えるようユーティリティクラスに隔離してもいいかもですね。
            var type = typeof(Category);
            var lookup = type.GetFields()
                .Where(fi => fi.FieldType == type)
                .SelectMany(fi => fi.GetCustomAttributes(false),
                    (fi, Attribute) => new { Category = (Category)fi.GetValue(null), Attribute })
                .ToLookup(a => a.Attribute.GetType());

            // キャッシュに突っ込む
            idCache = lookup[typeof(IdAttribute)].ToDictionary(a => a.Category, a => (IdAttribute)a.Attribute);
            labelCache = lookup[typeof(LabelAttribute)].ToDictionary(a => a.Category, a => (LabelAttribute)a.Attribute);
            codeCache = lookup[typeof(CodeAttribute)].ToDictionary(a => a.Category, a => (CodeAttribute)a.Attribute);
            categoryIdCache = lookup[typeof(CategoryIdAttribute)].ToDictionary(a => a.Category, a => (CategoryIdAttribute)a.Attribute);
        }

        public static int ToId(this Category category)
        {
            return idCache[category].Value;
        }


        public static string ToLable(this Category category)
        {
            return labelCache[category].Value;
        }

        public static string ToCode(this Category category)
        {
            return codeCache[category].Value;
        }

        public static int ToCategoryId(this Category category)
        {
            return categoryIdCache[category].Value;
        }

        public static Category toCategory(int categoryId)
        {
            Category category = Category.None;
            foreach (KeyValuePair<Category, IdAttribute> pair in idCache)
            {
                if (pair.Value.Value == categoryId)
                {
                    category = pair.Key;
                }
            }
            return category;
        }

        public static Category toCategory(string label)
        {
            Category category = Category.None;
            foreach( KeyValuePair<Category,LabelAttribute> pair in labelCache)
            {
                if(pair.Value.ToString() == label)
                {
                    category = pair.Key;
                }
            }
            return category;
        }

        public static string toString(this Category category)
        {
            return ToLable(category);
        }


    }

    // 属性など
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class IdAttribute : Attribute
    {
        public int Value { get; private set; }

        public IdAttribute(int value)
        {
            Value = value;
        }
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class LabelAttribute : Attribute
    {
        public string Value { get; private set; }

        public LabelAttribute(string value)
        {
            Value = value;
        }
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class CodeAttribute : Attribute
    {
        public string Value { get; private set; }

        public CodeAttribute(string value)
        {
            Value = value;
        }
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class CategoryIdAttribute : Attribute
    {
        public int Value { get; private set; }

        public CategoryIdAttribute(int value)
        {
            Value = value;
        }
    }

}

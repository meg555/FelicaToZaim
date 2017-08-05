using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Newtonsoft.Json;
using System.Net;
using System.IO;

// http://deanhume.com/home/blogpost/a-simple-guide-to-using-oauth-with-c-/49
// https://api.codeproject.com/Samples/ClientCredCsDoc
// http://kuroeveryday.blogspot.jp/2013/04/ctwitteroauth.html

using OAuth;
namespace FelicaToZaim
{
    class ZaimClient
    {

        public ZaimClient()
        {
        }

        const string ruby_path = @"C:/tools/ruby24/bin/ruby.exe";
        const string rb_make_paymennt = @"ruby/make_payment.rb";

        private static string execCommand(string command, string args)
        {
            //Processオブジェクトを作成
            System.Diagnostics.Process p = new System.Diagnostics.Process();

            //ComSpec(cmd.exe)のパスを取得して、FileNameプロパティに指定
            p.StartInfo.FileName = command; // System.Environment.GetEnvironmentVariable("ComSpec");
            //出力を読み取れるようにする
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = false;
            p.StartInfo.RedirectStandardError = true;
            //ウィンドウを表示しないようにする
            p.StartInfo.CreateNoWindow = true;
            //コマンドラインを指定（"/c"は実行後閉じるために必要）
            //p.StartInfo.Arguments = string.Format(@"/c {0} /w", args);
            p.StartInfo.Arguments = string.Format(@"{0}", args);

            //起動
            p.Start();

            //出力を読み取る
            string results = p.StandardOutput.ReadToEnd();
            string error = p.StandardError.ReadToEnd();

            //プロセス終了まで待機する
            //WaitForExitはReadToEndの後である必要がある
            //(親プロセス、子プロセスでブロック防止のため)
            p.WaitForExit();
            p.Close();
            Console.WriteLine(error);

            return results;
        }


        public static void make_payment( DisplayItem item)
        {
            string date = convert_date_string(item.Date);

            string args = rb_make_paymennt + string.Format(" {0} {1} {2} '{3}' {4} '{5}' '{6}' '{7}'",
                 item.category.ToCategoryId(),
                 item.category.ToId(),
                 item.Price,
                 date,
                 4115403,
                 toSafeString("(FelicaToZaim投稿)"),
                 toSafeString(""),
                 toSafeString(item.ShopName)
                 );
            string result = execCommand(ruby_path, args);
            Console.WriteLine(result);

        }

        private static string toSafeString(string src)
        {
            return src;
        }
        
        private static string convert_date_string(string datestr)
        {

            //Regexオブジェクトを作成
            System.Text.RegularExpressions.Regex r =
                new System.Text.RegularExpressions.Regex(
                    @"(?<year>\d\d)/(?<month>\d\d)/(?<date>\d\d)(?<time>\s*).*",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            //TextBox1.Text内で正規表現と一致する対象を1つ検索
            System.Text.RegularExpressions.Match m = r.Match(datestr);
            

            int year = int.Parse(m.Groups["year"].Value);
            int month = int.Parse(m.Groups["month"].Value);
            int date = int.Parse(m.Groups["date"].Value);
            
            

            return string.Format("20{0:00}-{1:00}-{2:00}", year, month, date);

        }
        
    }
}

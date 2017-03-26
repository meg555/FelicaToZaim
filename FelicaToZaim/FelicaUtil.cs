using FelicaLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaToZaim
{
    /**
     * from http://tomosoft.jp/design/?p=4422
     * 
     */

    class FelicaUtil
    {

        // サービスコード
        public const int SERVICE_SUICA_INOUT = 0x108f;
        public const int SERVICE_SUICA_HISTORY = 0x090f;


        public static FelicaData suica_dump_history(Felica f, int i)
        {

            FelicaData fdata = new FelicaData();

            byte[] data;
            int ctype, proc, date, time, balance, seq, region;
            int in_line, in_sta, out_line, out_sta;
            int yy, mm, dd;

            data = f.ReadWithoutEncryption(SERVICE_SUICA_HISTORY, i);
            if (data == null)
            {
                return null;
            }

            ctype = data[0];            // 端末種
            proc = data[1];             // 処理
            date = (data[4] << 8) + data[5];        // 日付
            balance = (data[10] << 8) + data[11];   // 残高
            balance = ((balance) >> 8) & 0xff | ((balance) << 8) & 0xff00;
            seq = (data[12] << 24) + (data[13] << 16) + (data[14] << 8) + data[15];
            region = seq & 0xff;        // Region
            seq >>= 8;                  // 連番

            out_line = -1;
            out_sta = -1;
            time = -1;

            switch (ctype)
            {
                case 0xC7:  // 物販
                case 0xC8:  // 自販機          
                    time = (data[6] << 8) + data[7];
                    in_line = data[8];
                    in_sta = data[9];
                    break;

                case 0x05:  // 車載機
                    in_line = (data[6] << 8) + data[7];
                    in_sta = (data[8] << 8) + data[9];
                    break;

                default:
                    in_line = data[6];
                    in_sta = data[7];
                    out_line = data[8];
                    out_sta = data[9];
                    break;
            }

            fdata.Tanmatu = consoleType(ctype);
            fdata.Proc = procType(proc);
            // 日付
            yy = date >> 9;
            mm = (date >> 5) & 0xf;
            dd = date & 0x1f;


            fdata.Date = yy.ToString() + "/" + mm.ToString() + "/" + dd.ToString() + " ";

            // 時刻
            if (time > 0)
            {
                int hh = time >> 11;
                int min = (time >> 5) & 0x3f;

                fdata.Date += hh.ToString() + ":" + min.ToString();
            }

            fdata.Iri = in_line.ToString("X") + "/" + in_sta.ToString("X");
            if (out_line != -1)
            {
                fdata.Iri = out_line.ToString("X") + "/" + out_sta.ToString("X");
            }
            fdata.Zandakav = balance;
            fdata.Renban = seq;


            return fdata;

        }

        private static string consoleType(int ctype)
        {
            switch (ctype)
            {
                case 0x03: return "清算機";
                case 0x05: return "車載端末";
                case 0x08: return "券売機";
                case 0x12: return "券売機";
                case 0x16: return "改札機";
                case 0x17: return "簡易改札機";
                case 0x18: return "窓口端末";
                case 0x1a: return "改札端末";
                case 0x1b: return "携帯電話";
                case 0x1c: return "乗継清算機";
                case 0x1d: return "連絡改札機";
                case 0xc7: return "物販";
                case 0xc8: return "自販機";
            }
            return "???";
        }

        private static string procType(int proc)
        {
            switch (proc)
            {
                case 0x01: return "運賃支払";
                case 0x02: return "チャージ";
                case 0x03: return "券購";
                case 0x04: return "清算";
                case 0x07: return "新規";
                case 0x0d: return "バス";
                case 0x0f: return "バス";
                case 0x14: return "オートチャージ";
                case 0x46: return "物販";
                case 0x49: return "入金";
                case 0xc6: return "物販(現金併用)";
            }
            return "???";
        }
    }
}

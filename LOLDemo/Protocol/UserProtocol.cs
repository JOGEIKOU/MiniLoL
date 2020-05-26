using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol
{
    public class UserProtocol
    {
        public const int INFO_CREQ = 0;                                  //自分のデータを取得
        public const int INFO_SRES = 1;                                   //自分のデータを戻す
        public const int CREATE_CREQ = 2;                           //カウンタークリエイト申し込み
        public const int CREATE_SRES = 3;                           //クリエイトの結果を戻す
        public const int ONLINE_CREQ = 4;                            //カウンターOnline
        public const int ONLINE_SRES = 5;                            //Onlineの結果を戻す
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol
{
    public class MatchProtocol
    {
        //マッチング開始
        public const int ENTER_CREQ = 0;
        public const int ENTER_SRES = 1;
        //マッチング閉じる
        public const int LEAVE_CREQ = 2;
        public const int LEAVE_SRES = 3;
        //ノーマルマッチング完璧、キャンピオン選び画面に入ると知らせ、アナウンサーで
        public const int ENTER_SELECT_BRO = 4;
    }
}

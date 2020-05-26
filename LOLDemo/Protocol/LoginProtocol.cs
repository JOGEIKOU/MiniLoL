using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol
{
    /// <summary>
    /// ログイン協議
    /// </summary>
    public class LoginProtocol
    {
        //クライアントログイン申し込み
        public const int LOGIN_CREQ = 0;
        //サーバーからクライアントにレスポンス　ログイン結果
        public const int LOGIN_SRES = 1;
        //クライアント登録を申し込み
        public const int REG_CREQ = 2;
        //サーバーからクライアントにレスポンス　登録結果
        public const int REG_SRES = 3;
    }
}

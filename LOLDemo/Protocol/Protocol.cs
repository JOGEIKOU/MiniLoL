﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol
{
    public class GameProtocol
    {
        public const byte TYPE_LOGIN = 0;                              //ログインモジュール
        public const byte TYPE_USER = 1;                               //ユーザーモジュール
        public const byte TYPE_MATCH = 2;                            //ノーマルマッチング
        public const byte TYPE_SELECT = 3;                          //キャンピオンを選び
        public const byte TYPE_FIGHT = 4;                              //戦闘モジュール
    }
}

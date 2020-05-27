using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    [Serializable]
    public class SelectModel
    {
        public int userId;                                                     //ユーサID
        public string name;                                                //カウンダ名前
        public int Champion;                                             //選んだキャンセル
        public bool Enter;                                                  //もう入った？
        public bool Ready;                                                //もう準備した？
    }
}

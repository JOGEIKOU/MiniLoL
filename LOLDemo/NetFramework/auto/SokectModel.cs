using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFramework.auto
{
    public class SokectModel
    {
        /// <summary>
        /// 一層：モジュールを区別するタイプ
        /// </summary>
        public byte type { get; set; }

        /// <summary>
        /// 二層：モジュール下の子モジュール
        /// </summary>
        public int area { get; set; }

        /// <summary>
        /// 三層：Current処理のロジック機能
        /// </summary>
        public int command { get; set; }

        /// <summary>
        /// Current処理のメッセージデータ
        /// </summary>
        public object message { get; set; }




        public SokectModel()
        {

        }
        public SokectModel(byte _type,int _area,int _command,object _message)
        {
            this.type = _type;
            this.area = _area;
            this.command = _command;
            this.message = _message;
        }

        public T GetMessage<T>()
        {
            return (T)message;
        }

    }
}

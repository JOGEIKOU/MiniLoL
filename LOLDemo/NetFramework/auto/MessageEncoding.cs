using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFramework.auto
{
    public class MessageEncoding
    {
        /// <summary>
        /// メッセージのシリアライズ
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public  static byte[] encode(object value)
        {
            SokectModel model = value as SokectModel;
            ByteArray bytearray = new ByteArray();
            bytearray.write(model.type);
            bytearray.write(model.area);
            bytearray.write(model.command);
            //メッセージ存在するか？？
            if(model.message != null)
            {
                bytearray.write(SerializeUtil.encode(model.message));
            }
        
            byte[] res = bytearray.getBuff();
            bytearray.Close();
            return res;
        }

        /// <summary>
        /// メッセージのディシリアライズ
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object decode(byte[] value)
        {
            ByteArray bytearray = new ByteArray(value);
            SokectModel model = new SokectModel();
            byte type;
            int area;
            int command;
            //三層協議に従って、データを読み込み
            bytearray.read(out type);
            bytearray.read(out area);
            bytearray.read(out command);
            model.type = type;
            model.area = area;
            model.command = command;
            //メッセージ存在するか？？
            if (bytearray.Readnable)
            {
                byte[] message;
                //残ったデータを読み込み
                bytearray.read(out message, bytearray.Length - bytearray.Position);
                model.message = SerializeUtil.decode(message);
            }
            bytearray.Close();
            return model;
        }
    }
}

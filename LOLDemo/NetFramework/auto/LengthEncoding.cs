using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NetFramework.auto
{
    public class LengthEncoding
    {
        /// <summary>
        /// en code opr 
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static byte[] encode(byte[] buff)
        {
            //メモリストリーム対象を作る
            MemoryStream ms = new MemoryStream();
            //二進数のストリーム対象を書き入れ
            BinaryWriter bw = new BinaryWriter(ms);
            //msg length を書き込み
            bw.Write(buff.Length);
            // msgの内容を書き込み
            bw.Write(buff);
            byte[] res = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(), 0, res, 0, (int)ms.Length);
            bw.Close();
            ms.Close();
            return res;
        }

        /// <summary>
        /// sticky length decode
        /// </summary>
        /// <param name="cache"></param>
        /// <returns></returns>
        public static byte[] decode(ref List<byte> cache)
        {
            if (cache.Count < 4) return null;

            //メモリストリーム対象を作る,そして、cache dateを書き込み
            MemoryStream ms = new MemoryStream(cache.ToArray());
            //二進数ストリーム対象を読み込み
            BinaryReader br = new BinaryReader(ms);
            //キャッシュの中にint方のｍｓｇ長さ
            int length = br.ReadInt32();
            //もしmsgの長さ＞キャッシュデータの長さ、メッセージの書き込む終了しません、
            if(length > ms.Length - ms.Position)
            {
                return null;
            }
            //正しいデータ長さを読み込み
            byte[] res = br.ReadBytes(length);
            //キャッシュクリア
            cache.Clear();
            //読み込んだデータをキャッシュに入れる
            cache.AddRange(br.ReadBytes((int)ms.Length - (int)ms.Position));
            br.Close();
            ms.Close();
            return res;
        }
    }//Class_end
}

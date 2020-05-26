using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetFramework
{
    /// <summary>
    /// User token obj
    /// </summary>
    public class UserToken
    {
        //user sokect
        public Socket conn;
        //Async receive network data obj
        public SocketAsyncEventArgs receiveSAEA;
        //Async send network data obj
        public SocketAsyncEventArgs sendSAEA;


        public LengthEncode LE;
        public LengthDecode LD;

        public encode Ecode;
        public decode Dcode;

        public delegate void SendProcess(SocketAsyncEventArgs e);
        public SendProcess sendProcess;

        public delegate void CloseProcess(UserToken token, string error);
        public CloseProcess closeProcess;

        public AbsHandlerCenter center;

        //data cache
        List<byte> cache = new List<byte>();

        private bool isReading = false;
        private bool isWriting = false;

        Queue<byte[]> writeQueue = new Queue<byte[]>();



        public UserToken()
        {
            receiveSAEA = new SocketAsyncEventArgs();
            sendSAEA = new SocketAsyncEventArgs();
            receiveSAEA.UserToken = this;
            sendSAEA.UserToken = this;
            //受ける対象のバッファを設置
            receiveSAEA.SetBuffer(new byte[1024], 0, 1024);
        }

        /// <summary>
        /// receive msg
        /// </summary>
        /// sticky 問題を解く
        /// <param name="buffer"></param>
        public void receive(byte[] buffer)
        {
            //メッセージをキャッシュに書き込み
            cache.AddRange(buffer);
            if(!isReading)
            {
                isReading = true;
                onData();
            }
        }

        /// <summary>
        /// do data in cache
        /// </summary>
        void onData()
        {
            //cache msg encode 
            byte[] buff = null;
            //stickyある場合、do sticky
            if(LD != null)
            {
                buff = LD(ref cache);
                if (buff == null)
                {
                    isReading = false;
                    return;
                }
            }
            else
            {
                if (cache.Count == 0)
                {
                    isReading = false;
                    return;
                }
                buff = cache.ToArray();
                cache.Clear();
            }

            if(Dcode ==null)
            {
                throw new Exception("message decode process is null");
            }

            //メッセージをディシリアライズ
            object msg = Dcode(buff);
            //teach app about receive msg
            center.MessageReceive(this, msg);
            //tail recursion
            onData();
        }

        /// <summary>
        /// write and send msg
        /// </summary>
        public void writed(byte[] value)
        {
            if(conn == null)
            {
                //アクセスも断絶された
                closeProcess(this, "プロセスを断絶された！！！");
                return;
            }
            writeQueue.Enqueue(value);
            if(!isWriting)
            {
                isWriting = true;
                onWrite();
            }
        }

        public void onWrite()
        {
            //送信メッセージのキュー判断
            if(writeQueue.Count == 0)
            {
                isWriting = false;
                return;
            }
            //firstメッセージを取る
            byte[] buff = writeQueue.Dequeue();
            //非同期対象の発送バッファデータを設置
            sendSAEA.SetBuffer(buff, 0, buff.Length);
            //非同期送信開始
            bool res = conn.SendAsync(sendSAEA);
            if(!res)
            {
                sendProcess(sendSAEA);
            }
        }

        public void writed()
        {
            //tail recursion
            onWrite();
        }

        public void Close()
        {
            try
            {
                writeQueue.Clear();
                cache.Clear();
                isReading = false;
                isWriting = false;
                conn.Shutdown(SocketShutdown.Both);
                conn.Close();
                conn = null;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
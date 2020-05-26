using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace NetFramework
{
    public class ServerStart
    {
        Socket server;

        int MaxClientCnt;                                                                     //最大クライアント数

        Semaphore acceptClients;
        UserToPool pool;

        public LengthEncode LE;
        public LengthDecode LD;

        public encode Ecode;
        public decode Dcode;

        /// <summary>
        /// メッセージ処理センター、外部APP層から入れる
        /// </summary>
        public AbsHandlerCenter center;

        /// <summary>
        /// Init Listener
        /// </summary>
        /// <param name="port"></param>
        public ServerStart(int max)
        {
            //instantiate listener obj
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //max link cnt
            MaxClientCnt = max;    
        }

        public void Start(int port)
        {
            //new token pool
            pool = new UserToPool(MaxClientCnt);
            //accept client cnt
            acceptClients = new Semaphore(MaxClientCnt, MaxClientCnt);

            for (int i = 0; i < MaxClientCnt; ++i)
            {
                UserToken token = new UserToken();
                //Init token info
                token.receiveSAEA.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Comleted);
                token.sendSAEA.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Comleted);
                token.LD = LD;
                token.LE = LE;
                token.Ecode = Ecode;
                token.Dcode = Dcode;
                token.sendProcess = ProcessSend;
                token.closeProcess = ClientClose;
                token.center = center;
                pool.push(token);
            }

            try
            {
                //listen current all IP adress port
                server.Bind(new IPEndPoint(IPAddress.Any, port));
                server.Listen(10);
                StartAccept(null);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        /// <summary>
        /// start listen Client
        /// </summary>
        public void StartAccept(SocketAsyncEventArgs e)
        {
            //もし入れるもの＝null　new client Listen event　or remove current client link
            if( e == null)
            {
                e = new SocketAsyncEventArgs();
                e.Completed += new EventHandler<SocketAsyncEventArgs>(Accept_Comleted);
            }
            else
            {
                e.AcceptSocket = null;
            }
            //cnt -1
            acceptClients.WaitOne();
            bool res =  server.AcceptAsync(e);
            //非同期処理判断　起動しないと直ぐに完成　or Accept_Comleted();
            if (!res)
            {
                ProcessAccept(e);
            }
        }

        /// <summary>
        /// 受ける方法
        /// </summary>
        /// <param name="e"></param>
        public void ProcessAccept(SocketAsyncEventArgs e)
        {
            //link token obj from pool for new user
            UserToken token = pool.pop();
            token.conn = e.AcceptSocket;
            //talk with app about new client link
            center.ClientConnect(token);
            //start msg arrive listener
            StartRecvive(token);
            //release current Asyn obj
            StartAccept(e);
        }

        /// <summary>
        /// 受ける終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Accept_Comleted(object sender , SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }
        
        public void StartRecvive(UserToken token)
        {
            try
            {
                //user link obj    start Async data receive
                bool res = token.conn.ReceiveAsync(token.receiveSAEA);
                //Async event start 判断
                if (!res)
                {
                    ProcessReceive(token.receiveSAEA);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// IO stream comleted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void IO_Comleted(object sender, SocketAsyncEventArgs e)
        {
           if(e.LastOperation == SocketAsyncOperation.Receive)
            {
                ProcessReceive(e);
            }
           else
            {
                ProcessSend(e);
            }
        }

        /// <summary>
        /// メッセージを受ける方法
        /// </summary>
        /// <param name="e"></param>
        public void ProcessReceive(SocketAsyncEventArgs e)
        {
            UserToken token = e.UserToken as UserToken;
            //network msg receive success?
            if(token.receiveSAEA.BytesTransferred > 0 && token.receiveSAEA.SocketError == SocketError.Success)
            {
                byte[] message = new byte[token.receiveSAEA.BytesTransferred];
                Buffer.BlockCopy(token.receiveSAEA.Buffer,0,message,0,token.receiveSAEA.BytesTransferred);
                //do receive msg
                token.receive(message);
                StartRecvive(token);
            }
            else
            {
                if(token.receiveSAEA.SocketError != SocketError.Success)
                {
                    ClientClose(token, token.receiveSAEA.SocketError.ToString());
                }
                else
                {
                    ClientClose(token, "クライアントからネットワークを切断します。");
                }
            }
        }

        /// <summary>
        /// メッセージを送る方法
        /// </summary>
        /// <param name="e"></param>
        public void ProcessSend(SocketAsyncEventArgs e)
        {
            UserToken token = e.UserToken as UserToken;
            if(e.SocketError != SocketError.Success)
            {
                ClientClose(token, e.SocketError.ToString());
            }
            else
            {
                //send msg success and callback
                token.writed();
            }
        }

        /// <summary>
        /// Client unlink
        /// </summary>
        /// <param name="token">unlink client</param>
        /// <param name="error">error 内容</param>
        public void ClientClose(UserToken token,string error)
        {
            if(token.conn != null)
            {
                lock(token)
                {
                    //teach app about that client unlinked
                    center.ClientClose(token, error);
                    token.Close();
                    //開放 for other user use
                    pool.push(token);
                    acceptClients.Release();           
                }
            }
        }



    }
}

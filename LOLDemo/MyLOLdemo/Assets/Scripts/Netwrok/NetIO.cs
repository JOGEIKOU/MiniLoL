using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class NetIO
{
    private static NetIO _Instance;
    private Socket _Socket;
    private string _Ip = "127.0.0.1";
    private int _Port = 6660;
    private byte[] _ReadBuff = new byte[1024];
    //data cache
    List<byte> cache = new List<byte>();

    public List<SocketModel> Listmessage = new List<SocketModel>();
    private bool isReading = false;
    /// <summary>
    /// シングルトンパターン
    /// </summary>
    public static NetIO Instance
    {
        get
        {
            if(_Instance == null)
            {
                _Instance = new NetIO();
            }
            return _Instance;
        }
    }

    private NetIO()
    {
        try
        {
            //クライアントアクセス対象
            _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //サーバーにアクセス
            _Socket.Connect(_Ip, _Port);
            //非同期メッセージ受ける(メッセージ直に_ReadBuffに書き込み)
            _Socket.BeginReceive(_ReadBuff,0,1024,SocketFlags.None, ReceiveCallBack,_ReadBuff);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    /// <summary>
    /// メッセージ受けるCallback
    /// </summary>
    /// <param name="ar"></param>
    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            //受けるメッセージの長さを取得する
            int length = _Socket.EndReceive(ar);
            byte[] message = new byte[length];
            Buffer.BlockCopy(_ReadBuff, 0, message, 0, length);
            cache.AddRange(message);
            if (!isReading)
            {
                isReading = true;
                onData();
            }
            //非同期メッセージ受ける(メッセージ直に_ReadBuffに書き込み) tail recursion
            _Socket.BeginReceive(_ReadBuff, 0, 1024, SocketFlags.None, ReceiveCallBack, _ReadBuff);
        }
        catch(Exception e)
        {
            Debug.Log("サーバーから断絶された！！！" + e.Message);
            _Socket.Close();
        }
    }

    /// <summary>
    /// メッセージ送信
    /// </summary>
    public void Write(byte type,int area,int command, object message)
    { 
        ByteArray ba = new ByteArray();
        ba.write(type);
        ba.write(area);
        ba.write(command);
        //シリアライズで書き込み
        if(message != null)
        {
            ba.write(SerializeUtil.encode(message));
        }
        ByteArray arr1 = new ByteArray();
        arr1.write(ba.Length);
        arr1.write(ba.getBuff());
        try
        {
            _Socket.Send(arr1.getBuff());
        }
        catch(Exception e)
        {
            Debug.Log("Network error , relogin please" + e.Message);
        }
    }

    /// <summary>
    /// do data in cache
    /// </summary>
    void onData()
    {
        //メッセージ長さdecode
        byte[] res = decode(ref cache);
        //メッセージ不足、次のメッセージで補充
        if(res == null)
        {
            isReading = false;
            return;
        }

        SocketModel message = msgdecode(res);
        if(message == null)
        {
            isReading = false;
            return;
        }
        //msg処理
        Listmessage.Add(message);

        //tail recursion
        onData();
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
        if (length > ms.Length - ms.Position)
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

    /// <summary>
    /// メッセージのディシリアライズ
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static SocketModel msgdecode(byte[] value)
    {
        ByteArray bytearray = new ByteArray(value);
        SocketModel model = new SocketModel();
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

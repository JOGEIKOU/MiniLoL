using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SerializeUtil : MonoBehaviour
{
    /// <summary>
    /// シリアライズ
    /// object-->byte
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static byte[] encode(object value)
    {
        //encodeメモリIO対象をクリエイト
        MemoryStream ms = new MemoryStream();
        //二進数シリアライズ対象を入れ
        BinaryFormatter bf = new BinaryFormatter();
        //obj対象を二進数データにして、キャッシュに入れる
        bf.Serialize(ms, value);
        byte[] res = new byte[ms.Length];
        Buffer.BlockCopy(ms.GetBuffer(), 0, res, 0, (int)ms.Length);
        ms.Close();
        return res;
    }

    /// <summary>
    /// ディシリアライズ
    /// byte-->object
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static object decode(byte[] value)
    {
        //encodeメモリIO対象をクリエイト
        MemoryStream ms = new MemoryStream(value);
        //二進数シリアライズ対象を入れ
        BinaryFormatter bf = new BinaryFormatter();
        //二進数データをオブレジェンド対象にして、キャッシュに入れる
        object res = bf.Deserialize(ms);
        ms.Close();
        return res;
    }
}

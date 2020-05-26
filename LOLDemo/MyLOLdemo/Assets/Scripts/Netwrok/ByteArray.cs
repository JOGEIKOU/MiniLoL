using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ByteArray
{
    MemoryStream ms = new MemoryStream();

    BinaryWriter bw;
    BinaryReader br;

    public void Close()
    {
        bw.Close();
        br.Close();
        ms.Close();
    }

    /// <summary>
    /// 初期データの構造を支える
    /// </summary>
    /// <param name="buff"></param>
    public ByteArray(byte[] buff)
    {
        ms = new MemoryStream(buff);
        bw = new BinaryWriter(ms);
        br = new BinaryReader(ms);
    }

    /// <summary>
    /// Currentデータのポインタを読んで、データをもらう
    /// </summary>
    public int Position
    {
        get { return (int)ms.Position; }
    }

    /// <summary>
    /// Currentデータの長さを取得する
    /// </summary>
    public int Length
    {
        get { return (int)ms.Length; }
    }

    /// <summary>
    /// Currentまだ読んでいないデータあるか
    /// </summary>
    public bool Readnable
    {
        get { return ms.Length > ms.Position; }
    }

    /// <summary>
    /// ディフォルトコンストラクタ
    /// </summary>
    public ByteArray()
    {
        bw = new BinaryWriter(ms);
        br = new BinaryReader(ms);
    }

    #region Write方法
    public void write(int value)
    {
        bw.Write(value);
    }
    public void write(byte value)
    {
        bw.Write(value);
    }
    public void write(bool value)
    {
        bw.Write(value);
    }
    public void write(string value)
    {
        bw.Write(value);
    }
    public void write(byte[] value)
    {
        bw.Write(value);
    }
    public void write(double value)
    {
        bw.Write(value);
    }
    public void write(float value)
    {
        bw.Write(value);
    }
    public void write(long value)
    {
        bw.Write(value);
    }
    #endregion

    #region Read方法
    public void read(out int value)
    {
        value = br.ReadInt32();
    }
    public void read(out byte value)
    {
        value = br.ReadByte();
    }
    public void read(out bool value)
    {
        value = br.ReadBoolean();
    }
    public void read(out string value)
    {
        value = br.ReadString();
    }
    public void read(out byte[] value, int length)
    {
        value = br.ReadBytes(length);
    }
    public void read(out double value)
    {
        value = br.ReadDouble();
    }
    public void read(out float value)
    {
        value = br.ReadSingle();
    }
    public void read(out long value)
    {
        value = br.ReadInt64();
    }
    #endregion


    public void repositoin()
    {
        ms.Position = 0;
    }

    /// <summary>
    /// データを取得する
    /// </summary>
    /// <returns></returns>
    public byte[] getBuff()
    {
        byte[] res = new byte[ms.Length];
        Buffer.BlockCopy(ms.GetBuffer(), 0, res, 0, (int)ms.Length);
        return res;
    }

}

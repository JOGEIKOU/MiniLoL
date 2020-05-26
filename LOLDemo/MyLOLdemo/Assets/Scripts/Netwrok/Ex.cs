using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ここで必ず静的のクラス
/// </summary>
public static class Ex
{
    /// <summary>
    /// MonoBehaviourの拡張（メッセージ送信メソッド）
    /// </summary>
    /// <param name="moon"></param>
    /// <param name="type"></param>
    /// <param name="area"></param>
    /// <param name="command"></param>
    /// <param name="message"></param>
    public static void WriteMessage(this MonoBehaviour moon, byte type, int area, int command, object message)
    {
        NetIO.Instance.Write(type, area, command, message);
    }
}

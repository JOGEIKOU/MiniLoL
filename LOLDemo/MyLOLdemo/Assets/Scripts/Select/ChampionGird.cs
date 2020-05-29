using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChampionGird : MonoBehaviour
{
    private Button btn;
    private Image img;
    private int id = -1;

    public void Init(int id)
    {
        this.id = id;
        img.sprite = Resources.Load<Sprite>("");
    }

    /// <summary>
    /// 操作許可
    /// </summary>
    public void Active()
    {
        btn.interactable = true;

    }

    /// <summary>
    /// 操作不可
    /// </summary>
    public void Deactive()
    {
        btn.interactable = false;
    }

    public void Click()
    {
        if(id!= -1)
        {
            //チャンピオン選択イベント
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
using Protocol.Dto;

public class SelectScene : MonoBehaviour
{
    [SerializeField]
    private GameObject ChampionBtton;
    //キャラクターの親節
    [SerializeField]
    private Transform ListParent;
    [SerializeField]
    private GameObject initMask;
    //レッドチーム
    [SerializeField]
    private SelectGrid[] red;
    //ブルーチーム
    [SerializeField]
    private SelectGrid[] blue;
    //チャンピオンIDとボタンのマップ関係
    private Dictionary<int, ChampionGird> gridMap = new Dictionary<int, ChampionGird>();

    void Start()
    {
        //1 mask表す　誤動作を避ける
        initMask.SetActive(true);

        //チャンピオンリスト初期化
        InitChampionList();

        //シーンをロードと「入る」と知らせ
        this.WriteMessage(GameProtocol.TYPE_SELECT, 0, SelectProtocol.ENTER_CREQ, null);
    }


    /// <summary>
    /// チャンピオンリスト初期化
    /// </summary>
    private void InitChampionList()
    {
        if (GameData.user == null) return;
        int index = 0;
        foreach (int i in GameData.user.championList)
        {
            //チャンピオンheadと選択リストをクリエイト
            GameObject btn = Instantiate<GameObject>(ChampionBtton);
            ChampionGird grid = btn.GetComponent<ChampionGird>();
            grid.Init(i);
            btn.transform.parent = ListParent;
            btn.transform.localScale = Vector3.one;
            btn.transform.localPosition = new Vector3(-390 + index * 110 * (index % 8), 155 + index / 8 * (-110), 0);
            gridMap.Add(i, grid);
        }
    }


    public void CloseMask()
    {
        initMask.SetActive(false);
    }

    private void RefreshUI(SelectRoomDTO roomDTO)
    {
        int team = roomDTO.GetTeam(GameData.user.id);
    }

}

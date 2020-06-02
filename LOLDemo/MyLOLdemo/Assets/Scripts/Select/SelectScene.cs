using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
using Protocol.Dto;
using UnityEngine.UI;

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
    //確認ボタン
    [SerializeField]
    private Button ready;
    //チャンピオンIDとボタンのマップ関係
    private Dictionary<int, ChampionGird> gridMap = new Dictionary<int, ChampionGird>();

    void Start()
    {
        SelectEventUtil.selected = Selected;
        SelectEventUtil.refreshUI = RefreshUI;
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
            btn.transform.localPosition = new Vector3(-390 +  110 * (index % 8), 155 + index / 8 * (-110), 0);
            gridMap.Add(i, grid);
            index++;
        }
    }


    public void CloseMask()
    {
        initMask.SetActive(false);
    }

    private void RefreshChampionList(SelectRoomDTO roomDTO)
    {
        int team = roomDTO.GetTeam(GameData.user.id);
        List<int> selected = new List<int>();
        if(team == 1)
        {
            foreach (SelectModel i in roomDTO.teamRed)
            {
                if(i.Champion != -1)
                {
                    selected.Add(i.Champion);
                }
            }
        }else
        {
            foreach (SelectModel i in roomDTO.teamBlue)
            {
                selected.Add(i.Champion);
            }
        }
        //選択されたチャンピオンのGridの状態設置
        foreach (int i in gridMap.Keys)
        {
            if(selected.Contains(i)  ||  !ready.interactable)
            {
                gridMap[i].Deactive();
            }
            else
            {
                gridMap[i].Active();
            }
        }
    }

    private void RefreshUI(SelectRoomDTO roomDTO)
    {
        int team = roomDTO.GetTeam(GameData.user.id);
        if(team == 1)
        {
            for (int i = 0; i < roomDTO.teamRed.Length; i++)
            {
                red[i].Refresh(roomDTO.teamRed[i]);
            }
            for (int i = 0; i < roomDTO.teamBlue.Length; i++)
            {
                blue[i].Refresh(roomDTO.teamRed[i]);
            }
        }
        else
        {
            for (int i = 0; i < roomDTO.teamRed.Length; i++)
            {
                blue[i].Refresh(roomDTO.teamRed[i]);
            }
            for (int i = 0; i < roomDTO.teamBlue.Length; i++)
            {
                red[i].Refresh(roomDTO.teamRed[i]);
            }
        }
        RefreshChampionList(roomDTO);
    }

    public void Selected()
    {
        ready.interactable = false;
    }

}

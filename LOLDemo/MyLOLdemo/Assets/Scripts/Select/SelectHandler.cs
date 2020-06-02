using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
using Protocol.Dto;
using UnityEngine.SceneManagement;

public class SelectHandler : MonoBehaviour,IHander
{

    private SelectRoomDTO roomDTO;

    public void MessageReceive(SocketModel model)
    {
        switch(model.command)
        {
            case SelectProtocol.DESTORY_BRO:
                //ロビーシーンに入る
                SceneManager.LoadScene(1);
                break;
            case SelectProtocol.ENTER_SRES:
                Enter(model.GetMessage<SelectRoomDTO>());
                break;
            case SelectProtocol.ENTER_EXBRO:
                Enter(model.GetMessage<int>());
                break;
            case SelectProtocol.FIGHT_BRO:
                //戦闘シーンに入る
                SceneManager.LoadScene(3);
                break;
            case SelectProtocol.READY_BRO:
                Ready(model.GetMessage<SelectModel>());
                break;
            case SelectProtocol.SELECT_BRO:
                Select(model.GetMessage<SelectModel>());
                break;
            case SelectProtocol.SELECT_SRES:
                WarningMsgMgr.Errorsobj.Add(new WarningModel("チャンピオン選択失敗した、もう一度選んでください。"));
                break;
            case SelectProtocol.TALK_BRO:
                Talk(model.GetMessage<string>());
                break;
        }
    }

    /// <summary>
    /// ユーサ(自分)に入る
    /// </summary>
    /// <param name="dto"></param>
    private void Enter(SelectRoomDTO dto)
    {
        roomDTO = dto;
        //UI refresh todo
        SelectEventUtil.refreshUI(roomDTO);
    }

    /// <summary>
    /// ユーサ（自分以外）に入る
    /// </summary>
    /// <param name="id"></param>
    private void Enter(int id)
    {
        if (roomDTO == null) return;
        //レッドチーム
        foreach (SelectModel i in roomDTO.teamRed)
        {
            if(i.userId == id)
            {
                i.Enter = true;
                //UI refresh 
                SelectEventUtil.refreshUI(roomDTO);
                return;
            }
        }
        //ブルーチーム
        foreach (SelectModel i in roomDTO.teamBlue)
        {
            if (i.userId == id)
            {
                i.Enter = true;
                //UI refresh 
                SelectEventUtil.refreshUI(roomDTO);
                return;
            }
        }
    }

    /// <summary>
    /// チャット機能
    /// </summary>
    /// <param name="value"></param>
    private void Talk(string value)
    {

    }

    private void Select(SelectModel model)
    {
        //レッドチーム
        foreach (SelectModel i in roomDTO.teamRed)
        {
            if (i.userId == model.userId)
            {
                i.Champion = model.Champion;
                //UI refresh 
                SelectEventUtil.refreshUI(roomDTO);
                return;
            }
        }
        //ブルーチーム
        foreach (SelectModel i in roomDTO.teamBlue)
        {
            if (i.userId == model.userId)
            {
                i.Champion = model.Champion;
                //UI refresh 
                SelectEventUtil.refreshUI(roomDTO);
                return;
            }
        }
    }


    private void Ready(SelectModel model)
    {
        if (model.userId == GameData.user.id)
        {
            //自分準備できたら、直ぐに状態変わると、クリックできない
            SelectEventUtil.selected();
        }
        //レッドチーム
        foreach (SelectModel i in roomDTO.teamRed)
        {
            if (i.userId == model.userId)
            {
                i.Champion = model.Champion;
                i.Ready = true;
                //UI refresh 
                SelectEventUtil.refreshUI(roomDTO);
                return;
            }
        }
        //ブルーチーム
        foreach (SelectModel i in roomDTO.teamBlue)
        {
            if (i.userId == model.userId)
            {
                i.Champion = model.Champion;
                i.Ready = true;
                //UI refresh 
                SelectEventUtil.refreshUI(roomDTO);
                return;
            }
        }
    }

}

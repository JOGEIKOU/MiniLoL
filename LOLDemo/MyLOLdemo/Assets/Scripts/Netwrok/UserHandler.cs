using Protocol;
using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserHandler : MonoBehaviour, IHander
{
    public void MessageReceive(SocketModel model)
    {
        switch(model.command)
        {
            case UserProtocol.INFO_SRES:
                AccountInfo(model.GetMessage<UserDTO>());
                break;
            case UserProtocol.CREATE_SRES:
                Create(model.GetMessage<bool>());
                break;
            case UserProtocol.ONLINE_SRES:
                UserOnline(model.GetMessage<UserDTO>());
                break;
        }
    }

    private void AccountInfo(UserDTO user)
    {
        if(user == null)
        {
            //召喚使いクリエイトpanelを表す
            SendMessage("OpenCreat");
        }
        else
        {
            //ログイン請求
            this.WriteMessage(GameProtocol.TYPE_USER, 0, UserProtocol.ONLINE_CREQ, null);
        }
    }

    private void UserOnline(UserDTO user)
    {
        GameData.user = user;
        //MaskPanel remove
        SendMessage("CloseMask");
        //UI data を　refresh
        SendMessage("RefreshView");
        WarningMsgMgr.Errorsobj.Add(new WarningModel("ログイン出来ました"));
    }

    private void Create(bool value)
    {
        Debug.Log(value.ToString());
        if(value)
        {
            WarningMsgMgr.Errorsobj.Add(new WarningModel("クリエイト出来ました",delegate
            {
                //CreatePanelを閉じる
                SendMessage("CloseCreat");
                //直接ログイン請求
                this.WriteMessage(GameProtocol.TYPE_USER, 0, UserProtocol.ONLINE_CREQ, null);
            }));
        }
        else
        {
            WarningMsgMgr.Errorsobj.Add(new WarningModel("クリエイト失敗しました", delegate
            {
                //refresh CreatePanel
                SendMessage("OpenCreat");
            }));

        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
using UnityEngine.SceneManagement;

public class LoginHandler : MonoBehaviour, IHander
{
    public void MessageReceive(SocketModel model)
    {
        switch (model.command)
        {
            case LoginProtocol.LOGIN_SRES:
                Login(model.GetMessage<int>());
                break;
            case LoginProtocol.REG_SRES:
                Register(model.GetMessage<int>());
                break;
        }
    }


    /// <summary>
    /// ログインreceive 処理
    /// </summary>
    private void Login(int value)
    {
        SendMessage("OpenLoginBtn");
        switch(value)
        {
            case 0:
                //WarningMsgMgr.Errorsobj.Add(new WarningModel("検証出来ました！！！"));
                //新しいシーンをロード
                SceneManager.LoadScene(1);
                break;
            case -1:
                WarningMsgMgr.Errorsobj.Add(new WarningModel("カウンター存在しない！！！"));
                break;
            case -2:
                WarningMsgMgr.Errorsobj.Add(new WarningModel("カウンター既にログイン中！！！"));
                break;
            case -3:
                WarningMsgMgr.Errorsobj.Add(new WarningModel("パスワード間違いました！！！"));
                break;
            case -4:
                WarningMsgMgr.Errorsobj.Add(new WarningModel("入力間違いました！！！"));
                break;
        }
    }

    /// <summary>
    /// 登録receive 処理
    /// </summary>
    private void Register(int value)
    {
        switch (value)
        {
            case 0:
                WarningMsgMgr.Errorsobj.Add(new WarningModel("ログイン出来ました"));
                //新しいシーンをロード
                break;
            case 1:
                WarningMsgMgr.Errorsobj.Add(new WarningModel("ログイン失敗しました、カウンター重複しました。"));
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Protocol;
using Protocol.Dto;

public class LoginScenes : MonoBehaviour {

    #region ログイン画面
    [SerializeField]
    private InputField _AccountInput;

    [SerializeField]
    private InputField _PasswordInput;
    #endregion

    #region 登録画面
    [SerializeField]
    private InputField _RegAccountInput;

    [SerializeField]
    private InputField _RegPasswordInput;

    [SerializeField]
    private InputField _RegPasswordReInput;

    [SerializeField]
    private Button _LoginBtn;

    [SerializeField]
    private GameObject _RegisterPanel;
    #endregion


    public void LoginOnClick()
    {
        if(_AccountInput.text.Length==0||_AccountInput.text.Length>6)
        {
            WarningMsgMgr.Errorsobj.Add(new WarningModel("ID間違っています！！！"));
            Debug.Log("ID間違っています！！！");
            return;
        }
        if (_PasswordInput.text.Length == 0 || _PasswordInput.text.Length > 6)
        {
            Debug.Log("Password間違っています！！！");
            return;
        }

        //_LoginBtn.enabled = false;
        //Debug.Log("ログイン出来た");
        //検証出来た、ログインを申し込み
        AccountInfoDTO dto = new AccountInfoDTO();
        dto.account = _AccountInput.text;
        dto.password = _PasswordInput.text;


        this.WriteMessage(GameProtocol.TYPE_LOGIN,0,LoginProtocol.LOGIN_CREQ,dto);
        _LoginBtn.interactable = false;
    }

    public void OpenLoginBtn()
    {
        _PasswordInput.text = string.Empty;
        _LoginBtn.interactable = true;
    }

    public void RegisterClick()
    {
        _RegisterPanel.SetActive(true);
    }

    public void RegisterExit()
    {
        _RegAccountInput.text = string.Empty;
        _RegPasswordInput.text = string.Empty;
        _RegPasswordReInput.text = string.Empty;
        _RegisterPanel.SetActive(false);
    }

    public void RegLogonReClick()
    {
        if (_RegAccountInput.text.Length == 0 || _RegAccountInput.text.Length > 6)
        {
            Debug.Log("ID間違っています！！！");
            return;
        }
        if (_RegPasswordInput.text.Length == 0 || _RegPasswordInput.text.Length > 6)
        {
            Debug.Log("Password間違っています！！！");
            return;
        }
        if (!_RegPasswordInput.text.Equals(_RegPasswordReInput.text))
        {
            Debug.Log("Password間違っています！！！");
            return;
        }

        //検証出来た、ログインを申し込み
        AccountInfoDTO dto = new AccountInfoDTO();
        dto.account = _RegAccountInput.text;
        dto.password = _RegPasswordInput.text;

        //検証出来た
        this.WriteMessage(GameProtocol.TYPE_LOGIN, 0, LoginProtocol.REG_CREQ, dto);
        RegisterExit();
    }

    private void test()
    {
        Debug.Log("CallbackTest");
    }



}

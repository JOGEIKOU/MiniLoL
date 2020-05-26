using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Protocol;

/// <summary>
/// 召喚使いPanel処理
/// </summary>
public class CreatePanel : MonoBehaviour
{
    [SerializeField]
    private InputField _NameField;

    [SerializeField]
    private Button _BtnOK;

    public void OpenPanel()
    {
        _BtnOK.enabled = true;
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void ClickListener()
    {
        if(_NameField.text.Length>6 || _NameField.text == string.Empty)
        {
            WarningMsgMgr.Errorsobj.Add(new WarningModel("正しい名前を書いてください"));
            return;
        }
        _BtnOK.enabled = false;
        this.WriteMessage(GameProtocol.TYPE_USER, 0, UserProtocol.CREATE_CREQ, _NameField.text);
    }

}

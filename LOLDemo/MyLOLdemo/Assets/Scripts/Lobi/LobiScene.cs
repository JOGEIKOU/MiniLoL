using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobiScene : MonoBehaviour
{
    //ユーザーの誤差しないように
    [SerializeField]
    private GameObject _MaskPanel;

    [SerializeField]
    private CreatePanel _CreatePanel;

    [SerializeField]
    private Text _MatchTxt;

    #region ユーザーのUIコーポレート定義

    [SerializeField]
    private Text _NameTxt;
    [SerializeField]
    private Slider _ExpBar;

    #endregion

    private void Start()
    {
        if(GameData.user == null)
        {
            _MaskPanel.SetActive(true);
            //サーバーへユーザーデータを請求
            this.WriteMessage(GameProtocol.TYPE_USER, 0, UserProtocol.INFO_CREQ, null);
        }
    }

    public void RefreshView()
    {
        _NameTxt.text = GameData.user.name + "　　レベル" +GameData.user.level;
        _ExpBar.value = GameData.user.exprience / 100;
    }


    /// <summary>
    /// open create panel
    /// </summary>
    private void OpenCreat()
    {
        _CreatePanel.OpenPanel();
    }

    /// <summary>
    ///  close create panel
    /// </summary>
    private void CloseCreat()
    {
        _CreatePanel.ClosePanel();
    }

    private void CloseMask()
    {
        _MaskPanel.SetActive(false);
    }

    public void NormalMatch()
    {
        if(_MatchTxt.text == "対戦待ち")
        {
            _MatchTxt.text = "キャンセル";
            this.WriteMessage(GameProtocol.TYPE_MATCH, 0, MatchProtocol.ENTER_CREQ, null);
        }
        else
        {
            _MatchTxt.text = "対戦待ち";
            this.WriteMessage(GameProtocol.TYPE_MATCH, 0, MatchProtocol.LEAVE_CREQ, null);
        }
    }
}

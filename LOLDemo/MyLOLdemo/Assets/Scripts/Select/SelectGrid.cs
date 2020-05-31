using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectGrid : MonoBehaviour
{
    [SerializeField]
    private Image _img_Grid;
    [SerializeField]
    private Text _txt_ChampionName;
    [SerializeField]
    private Image _img_Head;


    public void Refresh(SelectModel model)
    {
        _txt_ChampionName.text = model.name;
        //入ったか
        if(model.Enter)
        {
            //チャンピオン選択したか
            if(model.Champion == -1)
            {
                _img_Head.sprite = Resources.Load<Sprite>("Icons/NULL");
            }
            else
            {
                _img_Head.sprite = Resources.Load<Sprite>("Icons" + model.Champion);
            }
        }
        else
        {
            _img_Head.sprite = Resources.Load<Sprite>("Icons/Unselect");
        }
        //準備したか
        if(model.Ready)
        {
            Selected();
        }
        else
        {
            _img_Grid.color = Color.white;
        }
    }


    private void Selected()
    {
        _img_Grid.color = Color.red;

    }






}

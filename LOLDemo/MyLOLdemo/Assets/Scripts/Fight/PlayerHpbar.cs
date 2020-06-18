using Protocol.Dto.FightDTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpbar : MonoBehaviour
{
    [SerializeField]
    private SpriteSlider hp;
    [SerializeField]
    private TextMesh nameTxt;


    private void Update()
    {
        if(transform.rotation != Camera.main.transform.rotation)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }

    public void InitHpbar(FightPlayerModel model)
    {
        hp.Value = model.hp / model.maxHp;
        nameTxt.text = model.name;
    }

    public void HpbarChange(float value)
    {
        hp.Value = value;
    }


}

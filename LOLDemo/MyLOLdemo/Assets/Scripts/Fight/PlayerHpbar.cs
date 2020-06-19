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
    [SerializeField]
    private SpriteRenderer sr;


    private void Update()
    {
        if(transform.rotation != Camera.main.transform.rotation)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }

    public void InitHpbar(FightPlayerModel model,bool isFriend)
    {
        Debug.Log(GetType()+ "InitHpbar");
        hp.Value = model.hp / model.maxHp;
        nameTxt.text = model.name;
        if(isFriend)
        {
            sr.color = new Color(255, 255, 255, 0);
        }
    }

    public void HpbarChange(float value)
    {
        hp.Value = value;
    }


}

using Protocol.Dto.FightDTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Protocol;

public class SkillGird : MonoBehaviour
{
    [SerializeField]
    private Image mask;
    [SerializeField]
    private  Image Background;

    public FightSkill skill;

    private bool sclied = false;

    private float maxTime = 0;
    private float currentTime = 0;

    [SerializeField]
    private Button LevelUpBtn;



    public void InitSkill(FightSkill skill)
    {
        this.skill = skill;
        Sprite sp = Resources.Load<Sprite>("Skillicon/1_" + skill.code);
        Background.sprite = sp;
        mask.gameObject.SetActive(true);
    }


    private void Update()
    {
        if(sclied)
        {
            currentTime -= Time.deltaTime;
            if(currentTime <=0)
            {
                currentTime = 0;
                sclied = false;
                mask.gameObject.SetActive(false);
            }
            mask.fillAmount = currentTime / maxTime;
        }
    }

    public void SkillChange(FightSkill skill)
    {
        this.skill = skill;
    }

    public void SetMask(int time)
    {
        //0なら、CDマスクを消す
        if(time == 0)
        {
            if(!sclied && skill.level > 0)
            {
                mask.gameObject.SetActive(false);
                return;
            }
            else
            {
                mask.gameObject.SetActive(true);
                return;
            }
        }

        maxTime = time;
        currentTime = time;
        mask.gameObject.SetActive(true);
        sclied = true;
    }

    /// <summary>
    /// get pointer
    /// </summary>
    public void PointerEnter()
    {
        //tipを表す
    }

    /// <summary>
    /// lose pointer
    /// </summary>
    public void PointerExit()
    {
        //tipを閉じる
    }

    public void PointerClick()
    {
        if(currentTime >0)
        {
            return;
        }
        FightScene.Instance.skill = skill.code;
    }

    public void SetBtnState(bool state)
    {
        LevelUpBtn.interactable = state;
    }

    public void LevelUp()
    {
        //サーバーへメッセージ送信、スキルレベルアップ請求
        this.WriteMessage(GameProtocol.TYPE_FIGHT, 0, FightProtocol.SKILL_UP_CREQ, skill.code);
    }
}

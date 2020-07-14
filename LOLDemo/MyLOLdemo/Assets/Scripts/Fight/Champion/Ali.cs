using Protocol;
using Protocol.Dto.FightDTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ali : ChamCtrl
{
    private Transform[] list;

    [SerializeField]
    private GameObject atkEffect;


    public override void Attack(Transform[] target)
    {
        this.list = target;
        if(state == AnimState.RUN)
        {
            agent.CompleteOffMeshLink();
        }
        transform.LookAt(target[0]);
        state = AnimState.ATTACK;
        anim.SetInteger("State", AnimState.ATTACK);
    }

    /// <summary>
    /// 攻撃終了(override)
    /// </summary>
    public override void AttackFinish()
    {
        //近距離攻撃のダメージ計算メッセージをサーバに
        //遠距離なら、パティクルループ
        foreach (Transform item in list)
        {
            GameObject go = Instantiate(atkEffect, transform.position + transform.up * 2, transform.rotation);
            //パティクル移動
            go.GetComponent<TargetSkill>().Init(list[0], -1, data.id);
            state = AnimState.IDLE;
            anim.SetInteger("State", AnimState.IDLE);
        }
    }

    public override void Skilled()
    {
        state = AnimState.IDLE;
        anim.SetInteger("State", AnimState.IDLE);
    }

    public override void Skill(int code, Transform[] targets,Vector3 pos)
    {
        if(state == AnimState.RUN)
        {
            agent.CompleteOffMeshLink();
        }

        switch(code)
        {
            case 1:
                transform.LookAt(pos);
                GameObject go = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Pre_Skill/ali"), transform.position + transform.up * 2, transform.rotation);
                go.GetComponent<Skill_Q>().Init(this, 2, 20);
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                return;
        }

        state = AnimState.SKILL;
        anim.SetInteger("State", AnimState.SKILL);
    }

    public override void BaseSkill(int code, Transform[] target, Vector3 pos)
    {
        SkillAtkModel atk = new SkillAtkModel();
        switch(code)
        {
            case 1:
                atk.skill = code;
                atk.position = new float[] { pos.x, pos.y, pos.z };
                atk.type = 1;
                this.WriteMessage(GameProtocol.TYPE_FIGHT, 0, FightProtocol.SKILL_CREQ, atk);
                break;
            case 2:
                //todo
                break;
            case 3:
                //todo
                break;
            case 4:
                //todo
                break;
            default:
                return;
        }
    }
}

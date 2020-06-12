using Protocol.Dto.FightDTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChamCtrl : MonoBehaviour
{
    public FightPlayerModel data;

    protected Animator anim;

    protected NavMeshAgent agent;

    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// 攻撃anim終了
    /// </summary>
    public void AttackFinish()
    {

    }

    /// <summary>
    /// チャンピオン移動
    /// </summary>
    public void ChamMove(Vector3 goalPos)
    {
        agent.ResetPath();
        agent.SetDestination(goalPos);
        anim.SetInteger("State", AnimState.RUN);
    }

    /// <summary>
    /// 攻撃請求
    /// </summary>
    /// <param name="target">目標群</param>
    public void Attack(Transform[] targets)
    {

    }

    /// <summary>
    /// スキル発動請求
    /// </summary>
    /// <param name="code">スキル種類</param>
    /// <param name="targets">目標群</param>
    public void Skill(int code , Transform[] targets)
    {

    }


}

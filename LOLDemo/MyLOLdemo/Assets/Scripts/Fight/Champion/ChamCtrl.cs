using Protocol.Dto.FightDTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChamCtrl : MonoBehaviour
{
    [HideInInspector]
    public FightPlayerModel data;

    protected Animator anim;

    protected NavMeshAgent agent;

    //war fog(mask culling plane)
    [SerializeField]
    private GameObject mask;
    //hp sliderbar
    [SerializeField]
    private PlayerHpbar hpbar;
    [SerializeField]
    private MeshRenderer head;

    private int state = AnimState.IDLE;

    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        switch(state)
        {
            case AnimState.RUN:
                if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance <= 0 && !agent.pathPending)
                {
                    state = AnimState.IDLE;
                    anim.SetInteger("State", AnimState.IDLE);
                }
                else
                {
                    if (agent.isOnOffMeshLink)
                    {
                        agent.CompleteOffMeshLink();
                    }
                }
                break;
        }
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
        state = AnimState.RUN;
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

    /// <summary>
    /// Set iswar fog culling
    /// </summary>
    /// <param name="state"></param>
    private void MaskState(bool state)
    {
        mask.SetActive(state);
    }

    public void InitPlayerData(FightPlayerModel data,int myteam)
    {
        Debug.Log(data.name);
        this.data = data;
        hpbar.InitHpbar(data,data.team == myteam);
        head.material.SetTexture("_MainTex", Resources.Load<Texture>("Icons/" + data.code));
        //もし自分のチームではない場合、マスクを消す
        if(data.team != myteam)
        {
            gameObject.layer = LayerMask.NameToLayer("enemy");
            mask.SetActive(false);
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("friend");
            Destroy(GetComponent<Ward>());
        }
    }
}

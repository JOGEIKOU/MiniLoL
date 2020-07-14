using Protocol;
using Protocol.Dto.FightDTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightHandler : MonoBehaviour,IHander
{
    FightRoomModel room;

    /// <summary>
    /// レッドチームタワーの初期点
    /// </summary>
    [SerializeField]
    private Transform[] redPoss;

    /// <summary>
    /// レッドチームチャンピオンの初期点
    /// </summary>
    [SerializeField]
    private Transform redStartPos;

    /// <summary>
    /// ブルーチームタワーの初期点
    /// </summary>
    [SerializeField]
    private Transform[] bluePoss;

    /// <summary>
    /// ブルーチームチャンピオンの初期点
    /// </summary>
    [SerializeField]
    private Transform blueStartPos;

    private Dictionary<int, ChamCtrl> models = new Dictionary<int, ChamCtrl>();

    public void MessageReceive(SocketModel model)
    {
        switch(model.command)
        {
            case FightProtocol.START_BRO:
                StartFight(model.GetMessage<FightRoomModel>());
                break;
            case FightProtocol.MOVE_BRO:
                ChamMove(model.GetMessage<MoveDTO>());
                break;
            case FightProtocol.ATTACK_BRO:
                Attack(model.GetMessage<AttackDTO>());
                break;
            case FightProtocol.DAMAGE_BRO:
                Damage(model.GetMessage<DamageDTO>());
                break;
            case FightProtocol.SKILL_UP_SRES:
                SkillLevelUp(model.GetMessage<FightSkill>());
                break;
            case FightProtocol.SKILL_BRO:
                Skill(model.GetMessage<SkillAtkModel>());
                break;
        }
    }

    /// <summary>
    /// 戦闘開始
    /// </summary>
    /// <param name="value"></param>
    private void StartFight(FightRoomModel value)
    {
        room = value;

        int myteam = -1;
        foreach (AbsFightModel item in value.teamRed)
        {
            if(item.id == GameData.user.id)
            {
                myteam = item.team;
            }
        }

        if(myteam == -1)
        {
            foreach (AbsFightModel item in value.teamBlue)
            {
                if (item.id == GameData.user.id)
                {
                    myteam = item.team;
                }
            }
        }


        Debug.Log("キャラクターと建築物初期化");
        //レッドチームのチャンピオンと建築物生成
        foreach(AbsFightModel i in value.teamRed)
        {
            GameObject go;
            ChamCtrl pc;
            if (i.type == ModelType.HUMAN)
            {
                Debug.Log("red champion");
                go = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Pre_Player/" + i.code),redStartPos.position + new Vector3(Random.Range(1,1),0,Random.Range(1,1)),redStartPos.rotation);
                pc = go.GetComponent<ChamCtrl>();
                pc.InitPlayerData((FightPlayerModel)i,myteam);
            }
            else
            {
                Debug.Log("red build");
                go = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Pre_Build/2_" + i.code), redPoss[i.code - 1].position, redPoss[i.code - 1].rotation);
                pc = go.GetComponent<ChamCtrl>();
            }
            this.models.Add(i.id, pc);
            if(i.id == GameData.user.id)
            {
                FightScene.Instance.InitView((FightPlayerModel)i,pc);
                FightScene.Instance.LookAtCham();
            }
        }

        //ブルーチームのチャンピオンと建築物生成
        foreach (AbsFightModel i in value.teamBlue)
        {
            GameObject go;
            ChamCtrl pc;
            if (i.type == ModelType.HUMAN)
            {
                Debug.Log("blue champion");
                go = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Pre_Player/" + i.code),blueStartPos.position + new Vector3(Random.Range(1, 1), 0, Random.Range(1, 1)), blueStartPos.rotation);
                pc = go.GetComponent<ChamCtrl>();
                pc.InitPlayerData((FightPlayerModel)i,myteam);
            }
            else
            {
                Debug.Log("blue build");
                go = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Pre_Build/1_" + i.code), bluePoss[i.code - 1].position, bluePoss[i.code - 1].rotation);
                pc = go.GetComponent<ChamCtrl>();
            }

            this.models.Add(i.id, pc);
            if (i.id == GameData.user.id)
            {
                FightScene.Instance.InitView((FightPlayerModel)i,pc);
                FightScene.Instance.LookAtCham();
            }
        }
    }

    /// <summary>
    /// チャンピオン移動
    /// </summary>
    /// <param name="value"></param>
    private void ChamMove(MoveDTO value)
    {
        Debug.Log("チャンピオン移動");
        Vector3 target = new Vector3(value.x,value.y,value.z);
        models[value.userId].SendMessage("ChamMove", target);
    }

    private void Damage(DamageDTO value)
    {
        foreach (int[] item in value.target)
        {
            ChamCtrl cc = models[item[0]];
            cc.data.hp -= item[1];
            //-HP
            FightScene.Instance.NumUp(cc.transform, item[1].ToString());
            cc.HpChange();
            if(cc.data.id == GameData.user.id)
            {
                FightScene.Instance.RefreshView(cc.data);
            }
            if(item[2]>0)
            {
                if(item[0]>=0)
                {
                    cc.gameObject.SetActive(false);
                    if(cc.data.id == GameData.user.id)
                    {
                        FightScene.Instance.dead = true;
                    }
                }
                else
                {
                    Destroy(cc.gameObject);
                }
            }
        }
    }

    private void Attack(AttackDTO dto)
    {
        ChamCtrl obj = models[dto.userId];
        ChamCtrl target = models[dto.targetId];
        obj.Attack(new Transform[] { target.transform });
    }

    private void SkillLevelUp(FightSkill skill)
    {
        for(int i = 0; i < FightScene.Instance.myChampion.data.skills.Length;i++)
        {
            if(FightScene.Instance.myChampion.data.skills[i].code == skill.code)
            {
                FightScene.Instance.myChampion.data.free -= 1;
                FightScene.Instance.myChampion.data.skills[i] = skill;
                FightScene.Instance.RefreshLevelUp();
                return;
            }
        }
    }

    private void Skill(SkillAtkModel model)
    {
        List<Transform> list = new List<Transform>();
        if(model.type == 0)
        {
            list.Add(models[model.target].transform);
        }
        models[model.userId].Skill(model.skill, list.ToArray(), new Vector3(model.position[0], model.position[1], model.position[2]));
        if(model.userId == GameData.user.id)
        {
            FightScene.Instance.SkillMask(model.skill);
        }
    }

}

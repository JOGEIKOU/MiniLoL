using Protocol;
using Protocol.Dto.FightDTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSkill : MonoBehaviour
{
    Transform target;

    int skill;
    int userId;

    public void Init(Transform target,int skill,int userId)
    {
        this.skill = skill;
        this.target = target;
        this.userId = userId;
    }

    void Update()
    {
        if(target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, 0.5f);
            if(Vector3.Distance(transform.position,target.position)<0.1f)
            {
                //サーバにダメージ目標を送信
                DamageDTO dto = new DamageDTO();
                dto.userId = userId;
                dto.skill = skill;
                dto.target = new int[][] { new int[] { target.GetComponent<ChamCtrl>().data.id } };
                this.WriteMessage(GameProtocol.TYPE_FIGHT, 0, FightProtocol.DAMAGE_CREQ, dto);
                Destroy(gameObject);
            }
        }
    }
}

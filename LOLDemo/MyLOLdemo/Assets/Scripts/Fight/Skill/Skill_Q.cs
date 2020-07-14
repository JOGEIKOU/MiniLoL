using Protocol;
using Protocol.Dto.FightDTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Q : TryAgainSkill
{
    private void OnCollisionEnter(Collision collision)
    {
        int target;
        if (collision.gameObject.layer == LayerMask.NameToLayer("enemy"))
        {
            target = collision.gameObject.GetComponent<ChamCtrl>().data.id;
        }
        else
        {
            return;
        }
        DamageDTO dto = new DamageDTO();
        dto.userId = GameData.user.id;
        dto.skill = 1;
        dto.target = new int[][] { new int[] { target } };
        this.WriteMessage(GameProtocol.TYPE_FIGHT, 0, FightProtocol.DAMAGE_CREQ, dto);
    }
}

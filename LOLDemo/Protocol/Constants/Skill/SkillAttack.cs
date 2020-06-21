using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Protocol.Dto.FightDTO;

namespace Protocol.Constants.Skill
{
    public class SkillAttack : ISkill
    {
        public void Damage(int level, ref AbsFightModel atk, ref AbsFightModel target, ref List<int[]> damages)
        {
            int value = atk.normalAtk - target.normalDef;
            value = value > 0 ? value : 1;
            target.hp = target.hp - value <= 0 ? 0 : target.hp - value;
            damages.Add(new int[] { target.id, value, target.hp == 0 ? 0 : 1 });
        }
    }
}

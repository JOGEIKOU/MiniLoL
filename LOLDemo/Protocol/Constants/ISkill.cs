using Protocol.Dto.FightDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants
{
    public interface ISkill
    {
        void Damage(int level, ref AbsFightModel atk, ref AbsFightModel target, ref List<int[]> damages);
    }
}

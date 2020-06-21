using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.FightDTO
{
    [Serializable]
    public class AttackDTO
    {
        //攻撃者のID
        public int userId;
        //攻撃される者のID
        public int targetId;
    }
}

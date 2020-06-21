using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.FightDTO
{
    [Serializable]
    public class DamageDTO
    {
        public int userId;
        public int skill;
        public int[][] target;
    }
}

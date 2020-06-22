using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.FightDTO
{
    [Serializable]
    public class SkillAtkModel
    {
        public int userId;
        public int type;
        public int skill;
        public float[] position;
        public int target;
    }
}

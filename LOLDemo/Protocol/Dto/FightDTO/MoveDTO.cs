using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.FightDTO
{
    [Serializable]
    public class MoveDTO
    {
        public int userId;
        public float x;
        public float y;
        public float z;
    }
}

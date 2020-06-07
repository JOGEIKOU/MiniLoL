using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.FightDTO
{
    [Serializable]
    public class FightRoomModel
    {
        public AbsFightModel[] teamRed;
        public AbsFightModel[] teamBlue;
    }
}

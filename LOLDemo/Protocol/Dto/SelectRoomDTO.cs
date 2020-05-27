using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    /// <summary>
    /// red陣とBlue陣
    /// </summary>
    [Serializable]
    public class SelectRoomDTO
    {
        public SelectModel[] teamRed;
        public SelectModel[] teamBlue;
    }
}

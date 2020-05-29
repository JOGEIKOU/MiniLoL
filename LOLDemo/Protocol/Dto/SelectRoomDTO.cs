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


        public int GetTeam(int uid)
        {
            foreach (SelectModel i in teamRed)
            {
                if (i.userId == uid) return 1;
            }
            foreach (SelectModel i in teamBlue)
            {
                if (i.userId == uid) return 2;
            }
            return -1;
        }






    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.FightDTO
{
    public class BuildModel : AbsFightModel
    {
        public bool born;                                                              //復活か
        public int bornTime;                                                         //復活時間
        public bool initiative;                                                        //攻撃か
        public bool infrared;                                                         //隠す物を見るか
    }
}

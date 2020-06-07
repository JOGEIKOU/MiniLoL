using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.FightDTO
{
    [Serializable]
    public class BuildModel : AbsFightModel
    {
        public bool born;                                                              //復活か
        public int bornTime;                                                         //復活時間
        public bool initiative;                                                        //攻撃か
        public bool infrared;                                                         //隠す物を見るか

        public BuildModel(int id,int code,int hp,int maxHp,int atk,int def,bool reborn,int rebornTime,bool initiative,bool infrared , string name)
        {
            this.id = id;
            this.code = code;
            this.hp = hp;
            this.maxHp = maxHp;
            this.normalAtk = atk;
            this.normalDef = def;
            this.born = reborn;
            this.bornTime = rebornTime;
            this.initiative = initiative;
            this.infrared = infrared;
            this.name = name;

        }
    }
}

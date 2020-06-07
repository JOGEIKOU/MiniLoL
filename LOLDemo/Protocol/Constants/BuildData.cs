
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants
{
    /// <summary>
    /// 建築物属性
    /// </summary>
    public class BuildData
    {
        public static readonly Dictionary<int, BuildDataModel> buildMap = new Dictionary<int, BuildDataModel>();

        static BuildData()
        {
            Create(1, "ネクサス", 5000, 0, 50, false, true, false, 0);
            Create(2, "タワー3", 3000, 200, 50, false, true, true, 30);
            Create(3, "タワー2", 2000, 150, 30, true, true, false, 0);
            Create(4, "タワー1", 1000, 100, 20, true, true, false, 0);
        }

        static void Create(int code,string name,int hp ,int atk, int def , bool initiative , bool infrared ,bool reborn, int reborntime)
        {
            BuildDataModel model = new BuildDataModel(code, name, hp, atk, def, initiative, infrared, reborn, reborntime);
            buildMap.Add(code, model);
        }

        public partial class BuildDataModel
        {
            public int code;                                            //主キー
            public int hp;                                                 //HP
            public int atk;                                               //攻撃力
            public int def;                                               //防御力
            public bool initiative;                                   //攻撃か
            public bool infrared;                                    //隠す物を見えるか
            public string name;                                      //名前
            public bool reborn;                                      //復活か
            public int rebornTime;                                 //復活時間

            public BuildDataModel(){ }
            public BuildDataModel(int code,string name,int hp,int atk,int def,bool initiative,bool infrared,bool reborn,int rebornTime)
            {
                this.code = code;
                this.hp = hp;
                this.atk = atk;
                this.def = def;
                this.initiative = initiative;
                this.infrared = infrared;
                this.name = name;
                this.reborn = reborn;
                this.rebornTime = rebornTime;
            }
        }
    }
}

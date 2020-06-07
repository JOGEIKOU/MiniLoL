using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.FightDTO
{
    [Serializable]
    public class AbsFightModel
    {
        public int id;                                                     //チャンピオンID（戦闘エリアの主キー）
        public ModelType type;                                  //単位のタイプ
        public int code;                                               //このモデルの主キー
        public int hp;                                                    //current hp
        public int maxHp;                                            //最大HP

        public int normalAtk;                                       //普通攻撃
        public int magicAtk;                                        //魔法攻撃
        public int normalDef;                                      //普通防御
        public int magicDef;                                       //魔法防御

        public string name;                                         //名前
        public float speed;                                          //移動速度
        public float atkSpeed;                                    //攻撃速度
        public float atkRange;                                    //攻撃距離
        public float visionRange;                               //視野範囲
    }
}

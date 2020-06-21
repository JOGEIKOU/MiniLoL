using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.FightDTO
{
    [Serializable]
    public class FightPlayerModel:AbsFightModel
    {
        public int mp;                                                      //mp
        public int maxmp;                                               //最大mp
        public int level;                                                    //レベル
        public int exp;                                                     //経験値
        public int free;                                                     //レベルアップポイント
        public int gold;                                                    //ゴルド
        public int[] itemequs;                                          //装備一覧
        public FightSkill[] skills;                                      //ユーザースキル

        public int SkillLevel(int code)
        {
            foreach (FightSkill item in skills)
            {
                if(item.code == code)
                {
                    return item.level;
                }
            }
            return -1;
        }
    }
}

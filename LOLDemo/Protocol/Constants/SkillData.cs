using Protocol.Dto.FightDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants
{
    [Serializable]
    public class SkillData
    {
        public static readonly Dictionary<int, SkillDataModel> skillMap = new Dictionary<int, SkillDataModel>();

        static SkillData()
        {
            Create(1, "幻惑のオーブ", "Passive: 敵ユニットにスキルが当たる毎に1スタック増加し(最大9スタック)、最大スタック時のActiveが敵ユニットに当たった時に全スタック消費して1Hit毎にHPを回復する。スキル使用毎に最大3スタックまで増加し、回復HPはLv1/6/11/16で増加する。回復HP(1Hit毎): 3 / 5 / 9 / 18(+0.09AP) \nActive: 指定方向に貫通するオーブを放ち当たった敵ユニットに魔法DMを与える。オーブは往路と復路でそれぞれに当たり判定があり、帰りの場合は魔法DMの代わりにTrueDMを与える。魔法, TrueDM: 40 / 65 / 90 / 115 / 140(+0.35AP)Cost: 65 / 70 / 75 / 80 / 85MNCD: 7sRange: 880", SkillType.CURSORPOS, SkillTarget.E_NO_BULID, skillLevelData(1, 0, 0, 0), skillLevelData(3, 7, 65, 880), skillLevelData(5, 7, 70, 880), skillLevelData(7, 7, 75, 880), skillLevelData(9, 7, 80, 880), skillLevelData(-1, 7, 85, 880));
            Create(2, "フォックスファイア", "Active: 周囲を回る3つの狐火(5s)を身に纏う。狐火は危険な誘惑を受けている敵チャンピオンか3秒以内のAA対象の敵ユニットを優先的に、付近の敵ユニットに自動的に突撃して魔法DMを与える。同一対象に複数当たった場合、2Hit目以降は本来の30%分の魔法DMを与える(同一対象に3Hitで合計160%の魔法DM)。魔法DM(1Hit目): 40 / 65 / 90 / 115 / 140(+0.3AP)最大魔法DM: 64 / 104 / 144 / 184 / 224(+0.48AP)Cost: 40MNCD: 9 / 8 / 7 / 6 / 5sRange: 700(通常時) / 725(対優先対象)", SkillType.SELF, SkillTarget.E_CHANPION, skillLevelData(1, 0, 0, 0), skillLevelData(3, 7, 65, 880), skillLevelData(5, 7, 70, 880), skillLevelData(7, 7, 75, 880), skillLevelData(9, 7, 80, 880), skillLevelData(-1, 7, 85, 880));
            Create(3, "チャーム", "Active: 指定方向に投げキッスを放ち当たった敵ユニットのダッシュ・リープを中断して魔法DMとCharm(65%)、危険な誘惑(20%, 3s)を付与する。危険な誘惑は、幻惑のオーブ(Q)の復路を含む自身の全スキルにより受けるDMが増加(20%)する。魔法DM: 60 / 90 / 120 / 150 / 180(+0.4AP)Charm効果時間: 1.4 / 1.55 / 1.7 / 1.85 / 2sCost: 70MNCD: 12sRange: 975", SkillType.CURSORPOS, SkillTarget.E_CHANPION, skillLevelData(1, 0, 0, 0), skillLevelData(3, 7, 65, 880), skillLevelData(5, 7, 70, 880), skillLevelData(7, 7, 75, 880), skillLevelData(9, 7, 80, 880), skillLevelData(-1, 7, 85, 880));
            Create(4, "スピリットラッシュ", "Active: 指定方向にリープし、敵チャンピオンを優先とした付近の敵ユニット3体までに魔法DMを与える。魔法DMは同一対象にはスキル使用毎に一度しか発動しない。このスキルは使用後10秒以内は最大2回までCost無しで追加使用できる(CD: 1s)。魔法DM(1Hit毎): 60 / 90 / 120(+0.35AP)最大魔法DM: 180 / 270 / 360(+1.05AP)効果範囲: 600Cost: 100MNCD: 130 / 105 / 80sRange: 450",SkillType.CURSORPOS, SkillTarget.E_CHANPION, skillLevelData(1, 0, 0, 0), skillLevelData(3, 7, 65, 880), skillLevelData(5, 7, 70, 880), skillLevelData(7, 7, 75, 880), skillLevelData(9, 7, 80, 880), skillLevelData(-1, 7, 85, 880));
        }

        static SkillLevelData skillLevelData(int level,int time,int mp,float range)
        {
            SkillLevelData data = new SkillLevelData(level, time, mp, range);
            return data;
        }
 

        static void Create(int code ,string name,string info ,SkillType type ,SkillTarget target, params SkillLevelData[] levels)
        {
            SkillDataModel model = new SkillDataModel(code, name, info, type, target, levels);
            skillMap.Add(code, model);
        }
    }

    [Serializable]
    public partial class SkillLevelData
    {
        public int Level;
        public int Time;
        public int Mp;
        public float Range;

        public SkillLevelData() { }
        public SkillLevelData(int level, int time, int mp, float range)
        {
            this.Level = level;
            this.Time = time;
            this.Mp = mp;
            this.Range = range;
        }
    }


    public partial class SkillDataModel
    {
        public int code;
        public SkillLevelData[] levels;
        public string name;
        public string info;
        public SkillTarget target;
        public SkillType type;

        public SkillDataModel() { }

        public SkillDataModel(int code, string name, string info, SkillType type, SkillTarget target, SkillLevelData[] levels)
        {
            this.code = code;
            this.levels = levels;
            this.name = name;
            this.info = info;
            this.target = target;
            this.type = type;
        }
    }
}

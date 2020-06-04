using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto.FightDTO
{
    public class FightSkill
    {
        public int code;
        public int level;
        public int nextLevel;                                         //スキルレベルアップは必要なレベル（プレイヤー）
        public int cdtime;                                              //CD時間　（ーーｍｓ）
        public string name;                                          //スキルの名前
        public float range;                                            //スキルの放す距離
        public string info;                                              //スキルの説明
        public SkillTarget skilltarget;
        public SkillType skilltype;
    }


    /// <summary>
    /// スキル効果受けるターゲット
    /// </summary>
    public enum SkillTarget
    {
        SELF,                                                               //自分を中心点から放す
        F_CHANPION,                                               //自分陣のチャンピオン単位
        F_NO_BULID,                                                 //自分陣の非建築物単位
        F_ALL,                                                             //自分陣の全ての単位

        E_CHANPION,                                                //敵陣のチャンピオン単位
        E_NO_BULID,                                                 //敵陣の非建築物単位
        E_S_Normal,                                                   //敵とジャンル野生モンスター単位
        N_F_ALL,                                                         //非自分陣の全ての単位
    }

    /// <summary>
    /// スキル発動の出発点
    /// </summary>
    public enum SkillType
    {
        SELF,                                                                 //自分
        TAEGET,                                                           //目標を中心に
        CURSORPOS,                                                 //カーソル位置を中心に
    }

    /// <summary>
    /// 戦闘モデルタイプ
    /// </summary>
    public enum ModelType
    {
        BUILD,                                                              //建築物
        HUMAN,                                                           //生物
    }


}

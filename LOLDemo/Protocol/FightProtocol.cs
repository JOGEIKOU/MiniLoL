using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol
{
    public class FightProtocol
    {
        public const int ENTER_CREQ = 0;                                              //ロード完了、Fightルームを入れる
        public const int START_BRO = 1;                                                  //全ての人が入り、戦闘開始

        public const int MOVE_CREQ = 2;                                                //移動申し込み
        public const int MOVE_BRO = 3;                                                   //全ての人に移動を知らせ

        public const int SKILL_UP_CREQ = 4;                                          //スキルレベルアップ
        public const int SKILL_UP_SRES = 5;                                           //結果を返す

        public const int ATTACK_CREQ = 6;                                              //攻撃
        public const int ATTACK_BRO = 7;                                                 //全ての人に攻撃を知らせ

        public const int DAMAGE_CREQ = 8;                                            //ダメージイベント請求
        public const int DAMAGE_BRO = 9;                                               //ブロードキャスト

        public const int SKILL_CREQ = 10;                                                   //スキル発動請求
        public const int SKILL_BRO = 11;                                                      //ブロードキャスト
    }
}

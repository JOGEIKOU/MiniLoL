using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Dto
{
    [Serializable]
    public class UserDTO
    {
        public int id;                                                           //ユーザーID
        public string name;                                               //ユーザーネーム
        public int level;                                                       //レベル
        public int exprience;                                             //経験値
        public int wincount;                                               //勝数
        public int losecount;                                              //負数
        public int rancount;                                                //逃げ数

        public UserDTO() { }
        public UserDTO(string name,int id,int level ,int win,int lose,int ran)
        {
            this.id = id;
            this.name = name;
            this.wincount = win;
            this.losecount = lose;
            this.rancount = ran;
            this.level = level;
        }
    }
}

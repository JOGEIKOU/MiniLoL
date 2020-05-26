using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLoLServer.Dao.Model
{
    public class UserModel
    {
        public int id;                                                           //ユーザーID
        public string name;                                               //ユーザーネーム
        public int level;                                                       //レベル
        public int exprience;                                             //経験値
        public int wincount;                                               //勝数
        public int losecount;                                              //負数
        public int rancount;                                                //逃げ数
        public int accountId;                                           //キャラクターのカウンターID

        public UserModel()
        {
            level = 0;
            exprience = 0;
            wincount = 0;
            losecount = 0;
            rancount = 0;
        }

    }
}

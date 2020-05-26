using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLoLServer.Logic.Match
{
    /// <summary>
    /// ノーマルマッチングルームモデル
    /// </summary>
    public class MatchRoom
    {
        public int id;                                                                      //ルーム唯一ID
        public int teamMax = 1;                                                   //一つチームの最大メンバー数
        public List<int> teamRed = new List<int>();                 //redの陣
        public List<int> teamBule = new List<int>();                 //buleの陣
    }
}

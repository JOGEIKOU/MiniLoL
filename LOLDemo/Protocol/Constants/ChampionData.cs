using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Constants
{
    public class ChampionData
    {
        public static readonly Dictionary<int, ChampionDataModel> chamMap = new Dictionary<int, ChampionDataModel>();

        /// <summary>
        /// 静的コンストラクタ（初期化）
        /// </summary>
        static ChampionData()
        {
            //          code  name  natkb ndefb   matkb mdefb  hpb     mpb  natka  ndefa  matka mdefa  hpa   mpa  ,spd    atkspd  atkrage vrage skill
            Create(  1,   "アリ" , 60      ,20      ,10      ,30        ,450   ,300   ,5         ,2        ,10      ,10        ,30   ,50    ,1          ,0.5f   　,6   　     ,1   　,1   ,2  ,3   ,4);
            Create(2, "アムム" , 70      ,30     ,   0     , 35       , 500  , 280  , 6        , 4        , 2        ,2         , 50  , 20   , 1          , 0.5f,      1,            1,       1, 2   , 3   , 4);
            


        }

        private static void Create(int code,string name,
             int natkBase, int ndefBase,int matkBase, int mdefBase, int hpBase, int mpBase,
             int natkArr, int ndefArr, int matkArr, int mdefArr, int hpArr, int mpArr,
             float mSpeed, float atkSpeed, float atkRange, float visionRange,params int[] skills)
        {
            ChampionDataModel model = new ChampionDataModel();
            model.code = code;
            model.name = name;

            model.natkBase = natkBase;
            model.ndefBase = ndefBase;
            model.matkBase = matkBase;
            model.mdefBase = mdefBase;
            model.hpBase = hpBase;
            model.mpBase = mpBase;

            model.natkArr = natkArr;
            model.ndefArr = ndefArr;
            model.matkArr = matkArr;
            model.mdefArr = mdefArr;
            model.hpArr = hpArr;
            model.mpArr = mpArr;

            model.mSpeed = mSpeed;
            model.atkSpeed = atkSpeed;
            model.atkRange = atkRange;
            model.visionRange = visionRange;
            model.skills = skills;

            chamMap.Add(code, model);
        }
    }
    public partial class ChampionDataModel
    {
        public int code;                                                                    //チャンピオンデータの主キー
        public string name;                                                             //名前

        public int natkBase;                                                              //基礎物理攻撃力
        public int ndefBase;                                                              //基礎物理防御
        public int matkBase;                                                             //基礎魔法攻撃力
        public int mdefBase;                                                             //基礎魔法防御力
        public int hpBase;                                                                  //基礎HP
        public int mpBase;                                                                 //基礎MP

        public int natkArr;                                                                     //毎回レベルアッププラスの物理攻撃力
        public int ndefArr;                                                                     //毎回レベルアッププラスの物理防御力
        public int matkArr;                                                                    //　　　　　　　　　　　　魔法攻撃力
        public int mdefArr;                                                                    //                                                魔法防御力
        public int hpArr;                                                                        //                                                            HP
        public int mpArr;                                                                       //                                                            MP

        public float mSpeed;                                                               //移動速度
        public float atkSpeed;                                                             //攻撃力速度

        public float atkRange;                                                              //攻撃範囲
        public float visionRange;                                                         //視界範囲

        public int[] skills;                                                                       //スキルの種類
    }
}

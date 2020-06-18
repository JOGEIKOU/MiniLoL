//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;



//public struct FOWMapPos
//{
//    public int x;
//    public int y;

//    public FOWMapPos(int x,int y)
//    {
//        this.x = x;
//        this.y = y;
//    }
//}


///// <summary>
///// screen war fog
///// </summary>
//public class FogOfWarEffect : MonoBehaviour
//{
//    public enum FogMaskType
//    {
//        /// <summary>
//        /// 高精度FOV
//        /// </summary>
//        AccurateFOV,
//        /// <summary>
//        /// 基礎FOV
//        /// </summary>
//        BasicFOV,
//        /// <summary>
//        /// 簡単円図
//        /// </summary>
//        Circular,
//    }

//    public static FogOfWarEffect Instance
//    {
//        get
//        {
//            if(instance == null)
//            {
//                instance = FindObjectOfType<FogOfWarEffect>();
//            }
//            return instance;
//        }
//    }

//    private static FogOfWarEffect instance;

//    /// <summary>
//    /// マスクタイプ
//    /// </summary>
//    public FogMaskType fogMaskType { get { return m_FogMaskType; } }
//    /// <summary>
//    /// war fog color(RGBはカラー、α探索した区域透明度)
//    /// </summary>
//    public Color fogColor { get { return m_FogColor; } }
//    /// <summary>
//    /// fog area   幅
//    /// </summary>
//    public float xSize { get { return m_XSize; } }
//    /// <summary>
//    /// fog area 高さ
//    /// </summary>
//    public float zSize { get { return m_ZSize; } }
//    /// <summary>
//    /// fog texture 幅
//    /// </summary>
//    public int texWidth { get { return m_TexWidth; } }
//    /// <summary>
//    /// fog texture 高さ
//    /// </summary>
//    public int texHeight { get { return m_TexHeight; } }

//    public Vector3 centerPosition { get { return m_CenterPosition; } }

//    public float heightRange { get { return m_HeightRange; } }

//    public Texture2D fowMaskTexture
//    {
//        get
//        {
//            if(m_Map != null)
//            {
//                return;
//            }
//        }
//    }






//    [SerializeField]
//    private FogMaskType m_FogMaskType;
//    [SerializeField]
//    private Color m_FogColor = Color.black;
//    [SerializeField]
//    private float m_XSize;
//    [SerializeField]
//    private float m_ZSize;
//    [SerializeField]
//    private int m_TexWidth;
//    [SerializeField]
//    private int m_TexHeight;
//    [SerializeField]
//    private Vector3 m_CenterPosition;
//    [SerializeField]
//    private float m_HeightRange;
//    /// <summary>
//    /// 模糊偏移量
//    /// </summary>
//    [SerializeField]
//    private float m_BlurOffset;
//    /// <summary>
//    /// 模糊迭代次数
//    /// </summary>
//    [SerializeField]
//    private int m_BlurInteration;
//    /// <summary>
//    /// 是否生成小地图蒙版
//    /// </summary>
//    private bool m_GenerateMinimapMask;

//    /// <summary>
//    /// 迷雾特效shader
//    /// </summary>
//    public Shader effectShader;
//    /// <summary>
//    /// 模糊shader
//    /// </summary>
//    public Shader blurShader;
//    /// <summary>
//    /// 小地图蒙版渲染shader
//    /// </summary>
//    public Shader minimapRenderShader;

//    /// <summary>
//    /// 预生成的地图FOV数据（如果为空则使用实时计算FOV）
//    /// </summary>
//    //public FOWPregenerationFOVMapData pregenerationFOVMapData;

//    /// <summary>
//    /// 战争迷雾地图对象
//    /// </summary>
//    private FOWMap m_Map;
//}

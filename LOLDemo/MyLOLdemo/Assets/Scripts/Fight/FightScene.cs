using Protocol;
using Protocol.Constants;
using Protocol.Dto.FightDTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightScene : MonoBehaviour
{
    public static FightScene Instance;

    #region UI
    /// <summary>
    /// UI head
    /// </summary>
    [SerializeField]
    private Image head;
    /// <summary>
    /// hp mp
    /// </summary>
    [SerializeField]
    private Slider hpSlider;
    [SerializeField]
    private Slider mpSlider;
    /// <summary>
    /// name
    /// </summary>
    [SerializeField]
    private Text nameTxt;

    /// <summary>
    /// skill icon
    /// </summary>
    [SerializeField]
    private SkillGird[] skills;

    /// <summary>
    /// level
    /// </summary>
    [SerializeField]
    private Text LevelText;

    #endregion

    //マウスleftclick　スキルID
    public int skill = -1;

    public ChamCtrl myChampion;

    private Camera mainCamera;

    private int cameraH;
    private int cameraV;
    public float cameraSpeed = 1f;

    public bool dead = false;
    [SerializeField]
    private Transform NumParent;

    void Start()
    {
        Instance = this;
        mainCamera = Camera.main;
        this.WriteMessage(GameProtocol.TYPE_FIGHT, 0, FightProtocol.ENTER_CREQ, null);
    }

    private void Update()
    {
        switch (cameraH)
        {
            case 1:
                if (mainCamera.transform.position.z < 150)
                {
                    mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z + cameraH);
                }
                break;
            case -1:
                if (mainCamera.transform.position.z > 0)
                {
                    mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z + cameraH);
                }
                break;
        }

        switch (cameraV)
        {
            case 1:
                if (mainCamera.transform.position.x < 160)
                {
                    mainCamera.transform.position = new Vector3(mainCamera.transform.position.x + cameraV, mainCamera.transform.position.y, mainCamera.transform.position.z);
                }
                break;
            case -1:
                if (mainCamera.transform.position.x > 40)
                {
                    mainCamera.transform.position = new Vector3(mainCamera.transform.position.x + cameraV, mainCamera.transform.position.y, mainCamera.transform.position.z);
                }
                break;
        }
    }

    public void NumUp(Transform p,string value)
    {
        GameObject hp = (GameObject)Instantiate(Resources.Load("Prefabs/Pre_UI/HP"));
        hp.GetComponent<Text>().text = value;
        hp.transform.parent = NumParent;
        hp.transform.localScale = Vector3.one;
        hp.transform.localPosition = Camera.main.WorldToScreenPoint(p.position) + new Vector3(30, 20);
    }

    /// <summary>
    /// 表すデータを初期化
    /// </summary>
    /// <param name="model"></param>
    public void InitView(FightPlayerModel model, ChamCtrl champion)
    {
        myChampion = champion;
        head.sprite = Resources.Load<Sprite>("Icons/" + model.code);
        hpSlider.value = model.hp / model.maxHp;
        nameTxt.text = ChampionData.chamMap[model.code].name;
        LevelText.text = model.level.ToString();
        int i = 0;
        foreach (FightSkill item in model.skills)
        {
            skills[i].InitSkill(item);
            i++;
        }
    }

    public void RefreshView(FightPlayerModel model)
    {
        hpSlider.value = model.hp / model.maxHp;
        LevelText.text = model.level.ToString();
    }


    public void RefreshLevelUp()
    {
        int i = 0;
        foreach (FightSkill item in myChampion.data.skills)
        {
            if (item.nextLevel <= myChampion.data.level)
            {
                if(myChampion.data.free > 0)
                {
                    skills[i].SetBtnState(true);
                }
                else
                {
                    skills[i].SetBtnState(false);
                }
            }
            else
            {
                skills[i].SetBtnState(false);
            }
            skills[i].SkillChange(item);
            skills[i].SetMask(0);
            i++;
        }
    }

    public void LookAtCham()
    {
        mainCamera.transform.position = myChampion.transform.position + new Vector3(-6, 8, 0);
    }

    /// <summary>
    /// カメラ水平移動
    /// </summary>
    /// <param name="dir"></param>
    public void CameraHMove(int dir)
    {
        if (cameraH != dir)
            cameraH = dir;
    }

    /// <summary>
    /// カメラ縦移動
    /// </summary>
    /// <param name="dir"></param>
    public void CameraVMove(int dir)
    {
        if (cameraV != dir)
            cameraV = dir;
    }

    /// <summary>
    /// 左クリック
    /// </summary>
    /// <param name="position"></param>
    public void leftClick(Vector2 position)
    {
        if (skill == -1) return;
        Ray ray = mainCamera.ScreenPointToRay(position);
        RaycastHit[] hits = Physics.RaycastAll(ray, 200);
        List<Transform> list = new List<Transform>();
        Vector3 tigger = Vector3.zero;
        for(int i = hits.Length-1;i>=0;i--)
        {
            RaycastHit item = hits[i];

            if(item.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                tigger = item.point;
            }
            list.Add(item.transform);
        }
        myChampion.BaseSkill(skill, list.ToArray(), tigger);
        skill = -1;
    }

    /// <summary>
    /// 右クリック
    /// </summary>
    /// <param name="position">ポジション</param>
    public void rightClick(Vector2 position)
    {
        Ray ray = mainCamera.ScreenPointToRay(position);
        RaycastHit[] hits = Physics.RaycastAll(ray, 200);
        for (int i = hits.Length - 1; i >= 0; i--)
        {
            RaycastHit item = hits[i];

            //敵の陣の場合、攻撃
            if (item.transform.gameObject.layer == LayerMask.NameToLayer("enemy"))
            {
                ChamCtrl ctrl = item.transform.GetComponent<ChamCtrl>();
                if (Vector3.Distance(myChampion.transform.position, item.transform.position) < ctrl.data.atkRange)
                {
                    //ルナーンの効果なら、data.idは敵のID配列
                    this.WriteMessage(GameProtocol.TYPE_FIGHT, 0, FightProtocol.ATTACK_CREQ, ctrl.data.id);
                    return;
                }
                continue;
            }

            //自分の陣の単位を無視
            //地面なら路線を探す
            if (item.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                MoveDTO dto = new MoveDTO();
                dto.x = item.point.x;
                dto.y = item.point.y;
                dto.z = item.point.z;
                this.WriteMessage(GameProtocol.TYPE_FIGHT, 0, FightProtocol.MOVE_CREQ, dto);
                return;
            }
        }
    }

    /// <summary>
    /// スキルマスク
    /// </summary>
    /// <param name="code">スキル主キー</param>
    public void SkillMask(int code)
    {
        foreach (SkillGird item in skills)
        {
            if(item.skill.code == code)
            {
                item.SetMask(item.skill.cdtime);
            }
        }
    }
}

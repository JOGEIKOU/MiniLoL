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

    private GameObject myChampion;

    private Camera mainCamera;

    private int cameraH;
    private int cameraV;
    public float cameraSpeed = 1f;

    void Start()
    {
        Instance = this;
        mainCamera = Camera.main;
        this.WriteMessage(GameProtocol.TYPE_FIGHT, 0, FightProtocol.ENTER_CREQ, null);
    }

    private void Update()
    {
        switch(cameraH)
        {
            case 1:
                if(mainCamera.transform.position.z < 150)
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

    /// <summary>
    /// 表すデータを初期化
    /// </summary>
    /// <param name="model"></param>
    public void InitView(FightPlayerModel model , GameObject champion)
    {
        myChampion = champion;
        head.sprite = Resources.Load<Sprite>("Icons/" + model.code);
        hpSlider.value = model.hp / model.maxHp;
        mpSlider.value = model.mp / model.maxmp;
        nameTxt.text = ChampionData.chamMap[model.code].name;
        LevelText.text = model.level.ToString();
        int i = 0;
        foreach (FightSkill item in model.skills)
        {
            skills[i].InitSkill(item);
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

    public void rightClick(Vector2 position)
    {
        Ray ray = mainCamera.ScreenPointToRay(position);
        RaycastHit[] hits = Physics.RaycastAll(ray, 200);
        foreach (RaycastHit item in hits)
        {
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

}

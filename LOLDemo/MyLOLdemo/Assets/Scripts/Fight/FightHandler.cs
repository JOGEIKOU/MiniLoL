using Protocol;
using Protocol.Dto.FightDTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightHandler : MonoBehaviour,IHander
{
    private FightRoomModel room;

    [SerializeField]
    private Transform[] redPoss;

    [SerializeField]
    private Transform redStartPos;

    [SerializeField]
    private Transform[] bluePoss;

    [SerializeField]
    private Transform blueStartPos;

    private Dictionary<int, GameObject> models = new Dictionary<int, GameObject>();

    public void MessageReceive(SocketModel model)
    {
        switch(model.command)
        {
            case FightProtocol.START_BRO:
                StartFight(model.GetMessage<FightRoomModel>());
                break;
            case FightProtocol.MOVE_BRO:
                ChamMove(model.GetMessage<MoveDTO>());
                break;
        }
    }

    /// <summary>
    /// 戦闘開始
    /// </summary>
    /// <param name="value"></param>
    private void StartFight(FightRoomModel value)
    {
        room = value;
        Debug.Log("キャラクターと建築物初期化");
        //レッドチームのチャンピオンと建築物生成
        foreach(AbsFightModel i in value.teamRed)
        {
            GameObject go;
            if (i.type == ModelType.HUMAN)
            {
                Debug.Log("red champion");
                go = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Pre_Player/" + i.code),redStartPos.position + new Vector3(Random.Range(1,1),0,Random.Range(1,1)),redStartPos.rotation);
                ChamCtrl cc = go.GetComponent<ChamCtrl>();
                cc.InitPlayerData((FightPlayerModel)i);
            }
            else
            {
                Debug.Log("red build");
                go = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Pre_Build/2_" + i.code), redPoss[i.code - 1].position, redPoss[i.code - 1].rotation);
            }
            this.models.Add(i.id, go);
            if(i.id == GameData.user.id)
            {
                FightScene.Instance.InitView((FightPlayerModel)i,go);
                FightScene.Instance.LookAtCham();
            }
        }

        //ブルーチームのチャンピオンと建築物生成
        foreach (AbsFightModel i in value.teamBlue)
        {
            GameObject go;
            if (i.type == ModelType.HUMAN)
            {
                Debug.Log("blue champion");
                go = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Pre_Player/" + i.code),blueStartPos.position + new Vector3(Random.Range(1, 1), 0, Random.Range(1, 1)), blueStartPos.rotation);
                ChamCtrl cc = go.GetComponent<ChamCtrl>();
                cc.InitPlayerData((FightPlayerModel)i);
            }
            else
            {
                Debug.Log("blue build");
                go = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/Pre_Build/1_" + i.code), bluePoss[i.code - 1].position, bluePoss[i.code - 1].rotation);
            }
            this.models.Add(i.id, go);
            if (i.id == GameData.user.id)
            {
                FightScene.Instance.InitView((FightPlayerModel)i,go);
                FightScene.Instance.LookAtCham();
            }
        }
    }

    private void ChamMove(MoveDTO value)
    {
        Debug.Log("チャンピオン移動");
        Vector3 target = new Vector3(value.x,value.y,value.z);
        models[value.userId].SendMessage("ChamMove", target);
    }
}

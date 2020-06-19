using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneProcess : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("OnPointerClick");
        switch(eventData.pointerId)
        {
            case PointerInputModule.kMouseLeftId:
                //todo
                break;
            case PointerInputModule.kMouseRightId:
                FightScene.Instance.rightClick(eventData.position);
                break;
        }
    }

    private void Update()
    {
        Vector3 pos = Input.mousePosition;
        if(pos.x<10)
        {
            //カメラ←移動
            FightScene.Instance.CameraHMove(1);
        }
        else if(pos.x>Screen.width-10)
        {
            //カメラ→移動
            FightScene.Instance.CameraHMove(-1);
        }
        else
        {
            FightScene.Instance.CameraHMove(0);
        }

        if (pos.y<10)
        {
            //カメラ↓移動
            FightScene.Instance.CameraVMove(-1);
        }
        else if(pos.y > Screen.height-10)
        {
            //カメラ↑移動
            FightScene.Instance.CameraVMove(1);
        }
        else
        {
            FightScene.Instance.CameraVMove(0);
        }

        if(Input.GetKey(KeyCode.Space))
        {
            FightScene.Instance.LookAtCham();
        }
    }
}

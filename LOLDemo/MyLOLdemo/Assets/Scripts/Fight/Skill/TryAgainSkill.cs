using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TryAgainSkill : MonoBehaviour
{
    public float maxDis;
    public float speed;
    private float dis = 0;
    private int state = 0;                   //0は前に発射、１は戻す
    protected ChamCtrl actor;        //魔法使い対象

    public void Init(ChamCtrl actor,float speed,float maxDis)
    {
        this.actor = actor;
        this.maxDis = maxDis;
        this.speed = speed;
    }

    private void FixedUpdate()
    {
        if (state == 0)
        {
            Vector3 t = Vector3.forward * speed * Time.fixedDeltaTime;
            transform.Translate(t);
            dis += t.z;
            if (dis >= maxDis) state = 1;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, actor.transform.position, 0.5f);
            if (Vector3.Distance(transform.position, actor.transform.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }
}

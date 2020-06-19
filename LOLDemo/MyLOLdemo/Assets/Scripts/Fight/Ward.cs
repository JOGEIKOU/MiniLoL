using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ward : MonoBehaviour
{
    List<GameObject> list = new List<GameObject>();

    [SerializeField]
    private GameObject head;
    [SerializeField]
    private GameObject hpbar;
    [SerializeField]
    private GameObject root;



    private void Update()
    {
        if(list.Count>0)
        {
            //隠す物か
            //todo

            if(!head.activeSelf)
            {
                head.SetActive(true);
            }
            if(!hpbar.activeSelf)
            {
                hpbar.SetActive(true);
            }
            if(!root.activeSelf)
            {
                root.SetActive(true);
            }
        }
        else
        {
            if (head.activeSelf)
            {
                head.SetActive(false);
            }
            if (hpbar.activeSelf)
            {
                hpbar.SetActive(false);
            }
            if (root.activeSelf)
            {
                root.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        ChamCtrl ctrl = col.gameObject.GetComponent<ChamCtrl>();
        if(ctrl)
        {
            if(ctrl.data.team != GetComponent<ChamCtrl>().data.team)
            {
                list.Add(col.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if(list.Contains(col.gameObject))
        {
            list.Remove(col.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningPanel : MonoBehaviour
{
    [SerializeField]
    private Text WarinTextMsg;

    WarningResult result;

    public void Active(WarningModel value)
    {
        WarinTextMsg.text = value.value;
        this.result = value.result;
        if(value.delay > 0)
        {
            Invoke("Close",value.delay);
        }
        gameObject.SetActive(true);
    }

    public void Close()
    {
        if(IsInvoking("Close"))
        {
            CancelInvoke("Close");
        }
        gameObject.SetActive(false);
        if(result != null)
        {
            result();
        }
    }

}

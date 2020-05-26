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
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        if(result != null)
        {
            result();
        }
    }

}

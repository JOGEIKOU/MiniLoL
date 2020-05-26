using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningMsgMgr : MonoBehaviour
{
    public static List<WarningModel> Errorsobj = new List<WarningModel>();

    [SerializeField]
    private WarningPanel WaringMsg;
    private void Update()
    {
        if(Errorsobj.Count>0)
        {
            WarningModel err = Errorsobj[0];
            Errorsobj.RemoveAt(0);
            WaringMsg.Active(err);
        }
        
    }

}

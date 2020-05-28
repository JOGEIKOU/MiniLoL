using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public class SelectScene : MonoBehaviour
{
    void Start()
    {
        this.WriteMessage(GameProtocol.TYPE_SELECT, 0, SelectProtocol.ENTER_CREQ, null);
    }


    void Update()
    {
        
    }
}

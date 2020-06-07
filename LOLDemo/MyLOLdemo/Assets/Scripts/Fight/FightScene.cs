using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightScene : MonoBehaviour
{
    void Start()
    {
        this.WriteMessage(GameProtocol.TYPE_FIGHT, 0, FightProtocol.ENTER_CREQ, null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

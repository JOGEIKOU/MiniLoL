using Protocol;
using Protocol.Dto.FightDTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightHandler : MonoBehaviour,IHander
{
    public void MessageReceive(SocketModel model)
    {
        switch(model.command)
        {
            case FightProtocol.START_BRO:
                break;
        }
    }


    private void StartFight(FightRoomModel value)
    {
        FightRoomModel val = value;
    }

}

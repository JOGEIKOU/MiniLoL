using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
using UnityEngine.SceneManagement;

public class MatchHandler : MonoBehaviour, IHander
{
    public void MessageReceive(SocketModel model)
    {
        if(model.command == MatchProtocol.ENTER_SELECT_BRO)
        {
            SceneManager.LoadScene(2);
        }
    }
}

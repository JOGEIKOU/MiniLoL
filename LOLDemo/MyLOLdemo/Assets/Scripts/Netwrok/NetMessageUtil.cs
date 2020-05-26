using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public class NetMessageUtil : MonoBehaviour
{
    private IHander login;
    private IHander user;
    private void Start()
    {
        login = GetComponent<LoginHandler>();
        user = GetComponent<UserHandler>();
    }

    void Update()
    {
        while(NetIO.Instance.Listmessage.Count > 0)
        {
            SocketModel model =  NetIO.Instance.Listmessage[0];
            StartCoroutine("MessageReceive", model);
            NetIO.Instance.Listmessage.RemoveAt(0);
        }
    }

    public void MessageReceive(SocketModel model)
    {
        switch (model.type)
        {
            case GameProtocol.TYPE_LOGIN:
                login.MessageReceive(model);
                break;
            case GameProtocol.TYPE_USER:
                user.MessageReceive(model);
                break;
        }
    }
}
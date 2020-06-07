using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public class NetMessageUtil : MonoBehaviour
{
    private IHander login;
    private IHander user;
    private IHander match;
    private IHander select;
    private void Start()
    {
        login = GetComponent<LoginHandler>();
        user = GetComponent<UserHandler>();
        match = GetComponent<MatchHandler>();
        select = GetComponent<SelectHandler>();

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
            case GameProtocol.TYPE_MATCH:
                match.MessageReceive(model);
                break;
            case GameProtocol.TYPE_SELECT:
                select.MessageReceive(model);
                break;
        }
    }
}
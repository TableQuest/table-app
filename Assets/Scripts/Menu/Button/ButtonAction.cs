using Newtonsoft.Json;
using SocketIOClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAction : ButtonAbstract
{
    string endpoint;
    public ButtonAction(string prefabPath, string globalID, string endpoint) : base(prefabPath, globalID)
    {
        this.endpoint = endpoint;
    }

    public override async void functionOnClick()
    {
        GameObject.Find("SocketClient").GetComponent<Socket>().sendDebug("BUTTON ACTION : before if : click " + endpoint);
        if (endpoint != "")
        {
            SocketIO client = GameObject.Find("TableQuests").GetComponent<InitializationSocket>()._client;
            await client.EmitAsync(endpoint, globalID);
            GameObject.Find("SocketClient").GetComponent<Socket>().sendDebug("BUTTON ACTION : emit click " +  endpoint);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

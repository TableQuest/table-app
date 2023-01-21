using Newtonsoft.Json;
using SocketIOClient;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class ButtonAction : ButtonAbstract
{
    string endpoint;
    private Random r = new Unity.Mathematics.Random(0x6E624EB7u);
    public ButtonAction(string prefabPath, string globalID, string endpoint) : base(prefabPath, globalID)
    {
        this.endpoint = endpoint;
    }

    public override async void functionOnClick()
    {
        GameObject.Find("SocketClient").GetComponent<Socket>().sendDebug("BUTTON ACTION : before if : click " + endpoint);
        if (endpoint == "dice")
        {
            var diceManager = GameObject.Find("DiceManager").GetComponent<DiceManager>();
            if (diceManager != null)
            {
                // diceManager;
            }
        }
        else if (endpoint != "")
        {
            SocketIO client = GameObject.Find("TableQuests").GetComponent<InitializationSocket>()._client;
            await client.EmitAsync(endpoint, globalID);
            GameObject.Find("SocketClient").GetComponent<Socket>().sendDebug("BUTTON ACTION : emit click " +  endpoint);
        }
    }
}

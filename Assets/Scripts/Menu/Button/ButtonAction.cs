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
        if (endpoint != "")
        {
            SocketIO client = GameObject.Find("TableQuests").GetComponent<InitializationSocket>()._client;
            await client.EmitAsync(endpoint, globalID);
        } else
        {
            Debug.Log("click on" + prefabPath);
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

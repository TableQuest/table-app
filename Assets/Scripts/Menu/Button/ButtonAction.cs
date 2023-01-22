using Newtonsoft.Json;
using SocketIOClient;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
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
        if (endpoint == "dice")
        {
            var diceManager = GameObject.Find("DiceManager").GetComponent<DiceManager>();
            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(Resources.Load<AudioClip>("Audio/Effects/dice"));
            if (diceManager != null)
            {
                if (diceManager.waitingSkill)
                {
                    diceManager.DiceRoll();
                }
                else
                {
                    diceManager.OpenPanel(this.globalID, 15);
                    diceManager.DiceRollWithoutTarget();
                }
            }
        }
        else if (endpoint != "")
        {
            SocketIO client = GameObject.Find("TableQuests").GetComponent<InitializationSocket>()._client;
            await client.EmitAsync(endpoint, globalID);
        }
    }
}

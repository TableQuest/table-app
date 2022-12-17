using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAction : ButtonAbstract
{
    string messageToServer;
    public ButtonAction(string prefabPath, string globalID, string messageToServer) : base(prefabPath, globalID)
    {
        this.messageToServer = messageToServer;
    }

    public override void functionOnClick()
    {
        Debug.Log("envois message to server");
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

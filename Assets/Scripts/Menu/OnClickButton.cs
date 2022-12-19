using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnClickButton : MonoBehaviour
{
    private int counter;
    public UnityAction call;
    public static int MAX_COUNTER = 1000;

    public void onClick()
    {
        GameObject.Find("SocketClient").GetComponent<Socket>().sendDebug("click button - " + "counter =" + counter);
        if (counter == 0)
        {
            
            GameObject.Find("SocketClient").GetComponent<Socket>().sendDebug("before call != null");
            if (call != null)
            {
                GameObject.Find("SocketClient").GetComponent<Socket>().sendDebug("ENTER click button");
                call.Invoke();
                counter = 1;
            }
        }
    }

    private void Update()
    {
        if (counter != 0)
        {
            counter += 1;
        }

        if (counter > MAX_COUNTER)
        {
            GameObject.Find("SocketClient").GetComponent<Socket>().sendDebug("reset counter");
            counter = 0;
        }
    }
}
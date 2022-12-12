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
        if (counter == 0)
        {
            call.Invoke();
            counter = 1;
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
            counter = 0;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MyClick : MonoBehaviour
{
    private int counter;
    public UnityAction call;
    public static int MAX_COUNTER = 200;
    
    public void TestClick()
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

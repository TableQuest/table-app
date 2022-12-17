using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ButtonAbstract : MonoBehaviour
{
    private int counter;
    public UnityAction call;
    public GameObject buttonObject;
    public static int MAX_COUNTER = 1000;
    public string prefabPath;
    public string globalID;

    public ButtonAbstract(string prefabPath, string globalID)
    {
        this.prefabPath = prefabPath;
        this.globalID = globalID;
    }

    protected ButtonAbstract(string prefabPath)
    {
        this.prefabPath = prefabPath;
    }

    public virtual void functionOnClick()
    {
        Debug.Log("Click on " + prefabPath);
    }

    private void Update()
    {
    }
}

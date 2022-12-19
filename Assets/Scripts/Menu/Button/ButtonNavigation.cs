using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonNavigation : ButtonAbstract
{
    int page;
    public ButtonNavigation(string prefabPath, string globalID, int page) : base(prefabPath, globalID)
    {
        this.page = page;
    }

    public override void functionOnClick()
    {
        GameObject.Find("SocketClient").GetComponent<Socket>().sendDebug("BUTTON NAVIGATION - " + page);
        MenuBuilder.DisplayPage(page, GameObject.Find("TableQuests").GetComponent<MenuManager>().GetMenuWithId(this.globalID));
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

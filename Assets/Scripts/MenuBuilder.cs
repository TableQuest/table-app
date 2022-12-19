using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MenuBuilder : MonoBehaviour
{
    public static void InstantiateButton(Menu menu)
    {
        foreach (List<ButtonAbstract> page in menu.listPagesButton)
        {
            int i = 0;
            foreach (ButtonAbstract button in page)
            {
                GameObject buttonObject = Instantiate(Resources.Load(button.prefabPath) as GameObject, new Vector3(), Quaternion.identity);
                buttonObject.transform.SetParent(menu.tangibleObject.transform);
                buttonObject.AddComponent<OnClickButton>();
                buttonObject.GetComponent<OnClickButton>().call = delegate { button.functionOnClick(); };
                float angle = i * Mathf.PI * 2f / (float)page.Count;
                float radius = 1.5f;
                buttonObject.transform.localPosition = new Vector3(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius, 0);
                buttonObject.transform.localScale = new Vector3(1, 1, 1);
                buttonObject.SetActive(false);
                button.buttonObject = buttonObject;
                i++;
            }
        }
    }



    

    public static List<ButtonAbstract>[] generatePages(string globalID, string jsonSkills)
    {
        List<ButtonAbstract> page1 = new List<ButtonAbstract> { new ButtonNavigation("Prefab/ButtonAction", globalID,1), new ButtonAction("Prefab/ButtonTorche", globalID,""), new ButtonAction("Prefab/ButtonMove", globalID, "playerMove") };
        List<ButtonAbstract> page2 = new List<ButtonAbstract> { new ButtonCombat("Prefab/ButtonCombat", globalID, jsonSkills), new ButtonAction("Prefab/ButtonDice", globalID,""), new ButtonNavigation("Prefab/ButtonReturn", globalID, 0) };
        List<ButtonAbstract> page3 = new List<ButtonAbstract> { new ButtonAction("Prefab/Button", globalID,""), new ButtonAction("Prefab/Button", globalID,"") };
        List<ButtonAbstract>[] listPages = {page1, page2, page3};
        return listPages;
    }


    public static void DisplayPage(int pageIndex, Menu menu)
    {
        foreach(ButtonAbstract button in menu.listPagesButton[pageIndex])
        {
            button.buttonObject.SetActive(true);
        }
        HidePage(menu.currentPage, menu);
        menu.currentPage = pageIndex;
    }

    public static void HidePage(int pageIndex, Menu menu)
    {
        foreach (ButtonAbstract button in menu.listPagesButton[pageIndex])
        {
            button.buttonObject.SetActive(false);
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

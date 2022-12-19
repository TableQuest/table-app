using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ButtonCombat : ButtonAbstract
{
    public bool display = false;
    List<ButtonAbstract> buttons = new List<ButtonAbstract>();
    string jsonSkills;
    bool initButton = false;

    public class Skill
    {
        public int id { get; set; }
        public string name { get; set; }
        public int manaCost { get; set; }
        public int range { get; set; }
        public int maxTarget { get; set; }
        public string type { get; set; }
        public int statModifier { get; set; }
        public bool healing { get; set; }
        public string image { get; set; }
    }

    public ButtonCombat(string prefabPath, string globalID, string jsonSkills) : base(prefabPath, globalID)
    {
        this.jsonSkills = jsonSkills;
    }

    private void InstantiateButtonSkills(string jsonSkills)
    {
        var skillList = JsonConvert.DeserializeObject<List<Skill>>(jsonSkills);
        int i = 0;
        foreach(Skill skill in skillList)
        {
            ButtonAction button = new ButtonAction(skill.image, globalID, "");
            GameObject buttonObject = Instantiate(Resources.Load("Prefab/skill") as GameObject, new Vector3(), Quaternion.identity);
            buttonObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(skill.image);
            buttonObject.transform.SetParent(this.buttonObject.transform);
            buttonObject.AddComponent<OnClickButton>();
            buttonObject.GetComponent<OnClickButton>().call = delegate { Debug.Log("click sur competence" + skill.name); };
            float angle = i * Mathf.PI * 2f / (float)skillList.Count;
            float radius = 1.5f;
            buttonObject.transform.localPosition = new Vector3(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius, 0);
            buttonObject.transform.localScale = new Vector3(1, 1, 1);
            buttonObject.SetActive(false);
            button.buttonObject = buttonObject;
            buttons.Add(button);
            i++;
        }
    }

    public override void functionOnClick()
    {
        if (!initButton)
        {
            InstantiateButtonSkills(jsonSkills);
            initButton = true;
        }
        foreach (ButtonAbstract button in buttons)
        {
            button.buttonObject.SetActive(!display);
        }
        display = !display;
    }



}

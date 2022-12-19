using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ButtonCombat : ButtonAbstract
{   
    public bool display = false;
    List<ButtonAbstract> buttons = new List<ButtonAbstract>();
    string jsonSkills;
    bool initButton = false;

    

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
            buttonObject.GetComponent<OnClickButton>().call = delegate 
            {
                clickSkill(this.globalID, skill);
            };
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


    private async void clickSkill(string playerId, Skill skill) {
        Debug.Log(playerId + " clicked on " + skill.name);
        Socket socket = GameObject.Find("SocketClient").GetComponent<Socket>();
        socket.client.On("clickSkill", (data) => {

            string str = data.GetValue<string>(0);

            socket._mainThreadhActions.Enqueue(() => 
            {
                Debug.Log(str);
                SkillUse skillUse = JsonConvert.DeserializeObject<SkillUse>(str);
                Debug.Log(skillUse.skill.id);
                foreach (Player potentialTarget in GameObject.Find("TableQuests").GetComponent<GameState>()._entityManager._players)
                {
                    if (skillUse.targetsId.Contains(potentialTarget.globalId)) {
                        var button = potentialTarget.tangibleObject.transform.GetChild(0);
                        button.gameObject.SetActive(true);
                        button.GetComponent<OnClickButton>().call = delegate { sendSkillUsage(skillUse.playerId, skillUse.skill, potentialTarget.globalId, button.gameObject); };
                    }
                }
            });
        });

        var myData = new 
        {
            playerId = playerId,
            skillId = skill.id,
            skillName = skill.name
        };

        await socket.client.EmitAsync("clickSkill", JsonConvert.SerializeObject(myData));
    }

    private async void sendSkillUsage(string playerId, Skill skill, string targetId, GameObject buttonValidate) {
        Socket socket = GameObject.Find("SocketClient").GetComponent<Socket>();
        Debug.Log(playerId);
        Debug.Log(skill.id);
        Debug.Log(targetId);
        var data = new 
        {
            playerId = playerId,
            skillId = skill.id,
            targetId = targetId
        };

        string jsonData = JsonConvert.SerializeObject(data);
        await socket.client.EmitAsync("useSkill", jsonData);
        buttonValidate.SetActive(false);
        buttonValidate.GetComponent<OnClickButton>().call = null;

    }

}

public class SkillUse {
    public string playerId;
    public Skill skill;
    public string[] targetsId;
}

public class Skill
{
    public int id;
    public string name;
    public int manaCost;
    public int range;
    public int maxTarget;
    public string type;
    public int statModifier;
    public bool healing;
    public string image;
}
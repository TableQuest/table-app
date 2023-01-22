using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{
    Entity entity;
    bool hasMana;
    float numberPourcentage = 1.0f;
    GameObject FillImage;
    GameObject HealthBar;
    GameObject ManaBar;
    GameObject Pion;
    public TurnOrderHandler turnOrderHandler;
    Color32 colorTurn = new Color32(255, 191, 45, 255);
    Color32 colorNotTurn = new Color32(214, 214, 214, 255);

    public void Initialize(Entity entity, bool hasMana)
    {
        this.entity = entity;
        this.hasMana = hasMana;
    }
    // Start is called before the first frame update
    void Start()
    {
        FillImage = gameObject.transform.GetChild(0).gameObject;
        HealthBar = gameObject.transform.GetChild(1).GetChild(0).gameObject;
        ManaBar = gameObject.transform.GetChild(2).GetChild(0).gameObject;
        Pion = gameObject.transform.GetChild(3).gameObject;
        if (!hasMana)
        {
            gameObject.transform.GetChild(2).gameObject.SetActive(false);
        }
        Debug.Log("Le nom de mon entity est " + entity.name);
        Pion.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Pion" + entity.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (entity.lifeMax != 0)
        {
            HealthBar.GetComponent<Image>().fillAmount = ((((float)entity.life / (float)entity.lifeMax) * 100.0f) / 100.0f) * numberPourcentage;
            if (hasMana)
            {
                ManaBar.GetComponent<Image>().fillAmount = ((((float)entity.mana / (float)entity.manaMax) * 100.0f) / 100.0f) * numberPourcentage;
            }
        }
        if(entity.life <= 0)
        {
            turnOrderHandler.removeEntity(entity);
            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(Resources.Load<AudioClip>("Audio/Effects/death_npc"));
            GameObject.Destroy(gameObject);
        }
        if(turnOrderHandler.getEntityTurn().globalId == entity.globalId)
        {
            FillImage.GetComponent<Image>().color = colorTurn;
        } else
        {
            FillImage.GetComponent<Image>().color = colorNotTurn;
        }
    }
}

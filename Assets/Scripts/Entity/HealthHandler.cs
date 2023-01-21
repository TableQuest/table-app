using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HealthHandler : MonoBehaviour
{
    Entity entity;
    bool hasMana;
    float numberPourcentage;

    public void Initialize(Entity entity, bool hasMana)
    {
        this.entity = entity;
        this.hasMana = hasMana;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!hasMana)
        {
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().fillAmount = 1.0f;
            gameObject.transform.GetChild(0).GetComponent<Image>().fillAmount = 1.0f;
            numberPourcentage = 1.0f;
        } else
        {
            numberPourcentage = 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.GetChild(0).position = entity.tangibleObject.transform.position;
        gameObject.transform.GetChild(1).position = entity.tangibleObject.transform.position;
        //   StartCoroutine(GetRequest(url + "/players/" + menu.globalId + "/skills", menu));
        if (entity.lifeMax != 0)
        {
            gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().fillAmount = ((((float)entity.life / (float)entity.lifeMax) * 100.0f) / 100.0f) * numberPourcentage;
            if (hasMana)
            {
                gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().fillAmount = ((((float)entity.mana / (float)entity.manaMax) * 100.0f) / 100.0f) * numberPourcentage;
            }
        }
    }
}

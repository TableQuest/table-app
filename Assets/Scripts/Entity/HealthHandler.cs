using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
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
        if (entity.name != null && gameObject.transform.localScale != new Vector3(1,1,1))
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(Resources.Load<AudioClip>("Audio/Effects/test"));

        }
        if (entity.tangibleObject != null)
        {
            gameObject.transform.GetChild(0).position = entity.tangibleObject.transform.position;
            gameObject.transform.GetChild(1).position = entity.tangibleObject.transform.position;
        } else
        {
            GameObject.Destroy(gameObject);
        }
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

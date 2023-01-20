using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HealthHandler : MonoBehaviour
{
    Entity entity;

    public void Initialize(Entity entity)
    {
        this.entity = entity;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.GetChild(0).position = entity.tangibleObject.transform.position;
        gameObject.transform.GetChild(1).position = entity.tangibleObject.transform.position;
        //   StartCoroutine(GetRequest(url + "/players/" + menu.globalId + "/skills", menu));
        if (entity.lifeMax != 0)
        {
            Debug.Log("PAS A 0 LOL");
            gameObject.transform.GetChild(0).GetComponent<Image>().fillAmount = (((entity.life / entity.lifeMax) * 100) / 100) * 0.5f;
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.Events;

public class Menu : MonoBehaviour
{
	public string globalId;
	public string id;
	public GameObject tangibleObject;
	public GameObject button;

	public Menu(string id)
	{
		this.id = id;
	}

	public void InstantiateMenu(Vector2 pos)
	{
		tangibleObject = Instantiate(Resources.Load("Prefab/Menu") as GameObject, new Vector3(pos.x, pos.y, -10), Quaternion.identity);
		
		button = Instantiate(Resources.Load("Prefab/Button") as GameObject, new Vector3(), Quaternion.identity);
		button.transform.SetParent(tangibleObject.transform);
		button.transform.localPosition = new Vector3(0, 1.4f, 0);
		button.transform.localScale = new Vector3(1, 1, 1);
		
		button.GetComponent<OSCCursorEvent>().OnClick.AddListener((delegate { Debug.Log("COUCOU CLICK"); }));

		var test = Instantiate(Resources.Load("Prefab/Button") as GameObject, new Vector3(), Quaternion.identity);
		test.transform.SetParent(tangibleObject.transform);
		test.transform.localPosition = new Vector3(1.2f, -1.2f, 0);
		test.transform.localScale = new Vector3(1, 1, 1);
		
		var test2 = Instantiate(Resources.Load("Prefab/Button") as GameObject, new Vector3(), Quaternion.identity);
		test2.transform.SetParent(tangibleObject.transform);
		test2.transform.localPosition = new Vector3(-1.2f, -1.2f, 0);
		test2.transform.localScale = new Vector3(1, 1, 1);
		
		
		tangibleObject.name = "Menu" + id;
	}

    public void Move(Vector2 pos)
    {
		tangibleObject.transform.position = new Vector3(pos.x, pos.y, -10);
    }
}


using System;
using UnityEngine;

public class Menu : ScriptableObject
{
	public string globalId;
	public string id;
	public GameObject tangibleObject;

	public Menu(string id)
	{
		this.id = id;
	}

	public void InstantiateMenu(Vector2 pos)
	{
		tangibleObject = Instantiate(Resources.Load("Prefab/Menu") as GameObject, new Vector3(pos.x, pos.y, -10), Quaternion.identity);
		tangibleObject.name = "Menu" + id;
	}

	public void clickAttack()
	{
		Debug.Log("test : " + this.id);
	}



    public void Move(Vector2 pos)
    {
		tangibleObject.transform.position = new Vector3(pos.x, pos.y, -10);
	}

	public void Rotate(float degree)
	{
		tangibleObject.transform.rotation = Quaternion.Euler(0, 0, degree);
	}
}


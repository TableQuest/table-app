﻿using System;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;

public class Menu : ScriptableObject
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
		tangibleObject.name = "Menu" + id;
		button = Instantiate(Resources.Load("Prefab/Button") as GameObject, new Vector3(), Quaternion.identity);
		button.transform.SetParent(tangibleObject.transform);
		button.transform.localPosition = new Vector3(0, 1.4f, 0);
		button.transform.localScale = new Vector3(1, 1, 1);
		button.transform.GetComponent<OnClickButton>().call = delegate { clickAttack(); };
		
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


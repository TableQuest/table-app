using System;
using UnityEngine;

public abstract class Entity
{
	public string id;
	public GameObject tangibleObject;

	public Entity(string id)
	{
		this.id = id;
	}

	public void Move(Vector2 pos)
	{
		tangibleObject.transform.position = new Vector3(pos.x, pos.y, -10);
	}
}


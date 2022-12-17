using System;
using UnityEngine;
public class Player : Entity
{
	public string globalId;
	public GameObject helpConnection;
	public Vector2 tilePosition;
	public bool postionCheck = true;
	
	public Player(string id, string globalId, Vector2 gridPosition) : base(id)
	{
		this.globalId = globalId;
	}
 
}


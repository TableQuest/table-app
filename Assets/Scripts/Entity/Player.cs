using System;
using UnityEngine;
public class Player : Entity
{
	public string globalId;
	public GameObject helpConnection;
	
	public bool postionCheck = true;
	
	public Player(string id, string globalId, Vector2 tilePosition) : base(id,globalId)
	{
		this.globalId = globalId;
		this.tilePosition = tilePosition;
	}
 
}


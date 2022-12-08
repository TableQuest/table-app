using System;
using UnityEngine;
public class Player : Entity
{
	public string globalId;

	public Player(string id, string globalId, Vector2 pos) : base(id)
	{
		this.globalId = globalId;
	}
 
}


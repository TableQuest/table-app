using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : Entity
{
    public string name;

	public string pawnCode;

	public Npc(string id, string name) : base(id, id)
	{
		this.name = name;
	}


 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : Entity
{
    public string name;
	
	public Npc(string id, string name) : base(id)
	{
		this.name = name;
	}


 
}

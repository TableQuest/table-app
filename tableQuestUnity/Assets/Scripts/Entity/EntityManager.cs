using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager
{

	List<Player> players;
    // NPC[] npcs;


    public EntityManager()
	{
		this.players = new List<Player>();
	}

    public bool Exists(string id)
    {
        throw new NotImplementedException();
    }

    internal void Move(string id, Vector2 pos)
    {
        throw new NotImplementedException();
    }
}


using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{

	List<Player> players;
    // NPC[] npcs;


    public EntityManager()
	{
		this.players = new List<Player>();
	}

    public bool Exists(string id)
    {
        foreach(Player player in players)
        {
            if(player.id == id)
            {
                return true;
            }
        }
        return false;
    }

    public void Move(string id, Vector2 pos)
    {
        foreach (Player player in players)
        {
            if (player.id == id)
            {
                player.Move(pos);
            }
        }
    }


    public void CreateNewPlayer(string id, Vector2 pos, string idMenu)
    {
        Player player = new Player(id, id + idMenu, pos);
        players.Add(player);
        player.tangibleObject = Instantiate(Resources.Load("Prefab/Player") as GameObject, new Vector3(pos.x, pos.y, -10), Quaternion.identity);
        player.tangibleObject.name = "Menu" + id;
    }
}


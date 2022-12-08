using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityManager : MonoBehaviour
{

	List<Player> _players;
    // NPC[] npcs;

    GridManager _grid;

    private float WIDTH_GRID_UNIT = 1/24f; // we're dividing the screen in a grid that is 25 tiles wide
    private float HEIGHT_GRID_UNIT = 1/15f; //same but 14 tiles high


    public EntityManager()
	{
		this._players = new List<Player>();
        _grid = GameObject.Find("GridManager").GetComponent<GridManager>();
	}

    public bool Exists(string id)
    {
        return _players.Where(p => p.id == id).Count() > 0;
    }

    public void Move(string id, Vector2 pos)
    {
        float xCoord = (int)(pos.x / WIDTH_GRID_UNIT);
        float yCoord = -(int)(pos.y / HEIGHT_GRID_UNIT) + 14 ;
        float xPosition = _grid.GetTileAtPosition(0, 0).GetWidth() * xCoord + _grid.GetTileAtPosition(0, 0).GetWidth() / 2;
        float yPosition = _grid.GetTileAtPosition(0, 0).GetHeight() * yCoord + _grid.GetTileAtPosition(0, 0).GetHeight() / 2;

        Vector2 entityNewPosition = new Vector2(xPosition,
                                                yPosition);
        GetPlayerWithId(id).Move(entityNewPosition);

    }

    public void Rotate(string id, float degree)
    {
        GetPlayerWithId(id).Rotate(degree);
    }

    //TODO create the same method for the NPCs once they're added, or merge both into 1 method
    public Player GetPlayerWithId(string id) {
        Predicate<Player> matchingId = delegate(Player currentPlayer) { return currentPlayer.id == id; };
        return _players.Find(matchingId);
    }


    public void CreateNewPlayer(string id, Vector2 pos, string idMenu)
    {
        Player player = new Player(id, id + idMenu, pos);
        _players.Add(player);
        player.tangibleObject = Instantiate(Resources.Load("Prefab/Player") as GameObject, new Vector3(pos.x, pos.y, -10), Quaternion.identity);
        player.tangibleObject.name = "Menu" + id;
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EntityManager : MonoBehaviour
{

	public List<Player> _players;
    // NPC[] npcs;

    GridManager _grid;

    private float WIDTH_GRID_UNIT = 1/24f; // we're dividing the screen in a grid that is 25 tiles wide
    private float HEIGHT_GRID_UNIT = 1/15f; //same but 14 tiles high
    public GameObject button;


    void Start()
	{
		_players = new List<Player>();
        _grid = GameObject.Find("GridManager").GetComponent<GridManager>();
	}

    public bool Exists(string id)
    {
        return _players.Where(p => p.id == id).Count() > 0;
    }

    public void Move(string id, Vector2 pos)
    {
        Vector2 entityNewPosition = GetCanvasPosition(pos);
        GetPlayerWithId(id).Move(entityNewPosition);

    }

    public Vector2 GetCanvasPosition(Vector2 pos)
    {
        float xCoord = (int)(pos.x / WIDTH_GRID_UNIT);
        float yCoord = -(int)(pos.y / HEIGHT_GRID_UNIT) + 14 ;
        float xPosition = _grid.GetTileAtPosition(0, 0).GetWidth() * xCoord + _grid.GetTileAtPosition(0, 0).GetWidth() / 2;
        float yPosition = _grid.GetTileAtPosition(0, 0).GetHeight() * yCoord + _grid.GetTileAtPosition(0, 0).GetHeight() / 2;
        return new Vector2(xPosition, yPosition);
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

    public Player GetPlayerWithGlobalId(string globalId)
    {
        // Predicate<Player> matchingId = delegate(Player currentPlayer) { return currentPlayer.globalId == globalId; };
        // return _players.Find(matchingId);
        Debug.Log("FindPlayer with ID : " + globalId);
        Debug.Log("players size : "+ _players.Count);
        foreach (var player in _players)
        {
            Debug.Log(player.globalId);
        }

        return null;
    }

    public void CreateNewPlayer(string id, Vector2 pos, string idMenu)
    {
        
        Player player = new Player(id,  idMenu + id, pos);
        _players.Add(player);
        player.tangibleObject = Instantiate(Resources.Load("Prefab/Player") as GameObject, new Vector3(pos.x, pos.y, -10), Quaternion.identity);
        player.tangibleObject.name = "Pawn" + id;
        GameObject helperConnection = Instantiate(Resources.Load("Prefab/textID") as GameObject,new Vector3(0,0,-5), Quaternion.identity);
        helperConnection.transform.SetParent(player.tangibleObject.transform);
        helperConnection.name = "helper" + player.globalId;
        helperConnection.GetComponent<TextMeshPro>().text = player.globalId;
        player.helpConnection = helperConnection;
    }

    public void RemoveHelper(string playerId)
    {
        var helper = GameObject.Find("helper" + playerId);
        if (helper != null)
        {
            Destroy(helper);
        }
        else
        {
            Debug.LogError("Helper : "+  playerId+ " doesn't exists !");
        }
    }

    public void validerAction()
    {
        Debug.Log("Action validé");
    }


}


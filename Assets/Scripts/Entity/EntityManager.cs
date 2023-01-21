using System;
using System.Collections.Generic;
using System.Linq;
using SocketIOClient;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EntityManager : MonoBehaviour
{

	public List<Player> _players;
    public List<Npc> _npcs;

    GridManager _grid;

    private float WIDTH_GRID_UNIT = 1/24f; // we're dividing the screen in a grid that is 25 tiles wide
    private float HEIGHT_GRID_UNIT = 1/15f; //same but 14 tiles high


    void Start()
	{
		_players = new List<Player>();
        _npcs = new List<Npc>();
        _grid = GameObject.Find("GridManager").GetComponent<GridManager>();
	}

    public List<Entity> getEntities()
    {
        List<Entity> entities = new List<Entity>();
        entities.AddRange(_players);
        entities.AddRange(_npcs);
        return entities;
    }



    public bool Exists(string id)
    {
        return _players.Where(p => p.id == id).Count() > 0 || _npcs.Where(n => n.pawnCode == id).Count() > 0;
    }

    public void Move(string id, Vector2 oscPos)
    {
        Entity entity = GetEntityWithId(id);
        Vector2 canvasPosition = GetCanvasPosition(oscPos);
        entity.Move(canvasPosition);
        entity.tilePosition = _grid.GetPosFromEntityPos(canvasPosition);
    }

    public Vector2 GetCanvasPosition(Vector2 oscPos)
    {
        float xCoord = (int)(oscPos.x / WIDTH_GRID_UNIT);
        float yCoord = -(int)(oscPos.y / HEIGHT_GRID_UNIT) + 14 ;
        float xPosition = Tile.WIDTH * xCoord + Tile.WIDTH / 2;
        float yPosition = Tile.HEIGHT * yCoord + Tile.HEIGHT / 2;
        return new Vector2(xPosition, yPosition);
    }

    public void Rotate(string id, float degree)
    {
        GetEntityWithId(id).Rotate(degree);
    }



    public Player GetPlayerWithId(string id) {
        Predicate<Player> matchingId = delegate(Player currentPlayer) { return currentPlayer.id == id; };
        return _players.Find(matchingId);
    }

    public Npc GetNPCWithId(string id) {
        Predicate<Npc> matchingId = delegate(Npc currentNpc) { return currentNpc.pawnCode == id; };
        return _npcs.Find(matchingId);
    }

    public Player GetPlayerWithGlobalId(string globalId)
    {
        Predicate<Player> matchingId = delegate(Player currentPlayer) { return currentPlayer.globalId == globalId; };
        return _players.Find(matchingId);
    }

    public Entity GetEntityWithId(string id) {
        Player entity = GetPlayerWithId(id);
        return (entity != null ? entity : GetNPCWithId(id));
    }

    public Entity GetEntityWithGlobalId(string id)
    {
        Player entity = GetPlayerWithGlobalId(id);
        return (entity != null ? entity : GetNPCWithId(id));
    }

    public void CreateNewPlayer(string id, Vector2 pos, string idMenu)
    {
        Player player = new Player(id,  idMenu + id, _grid.GetPosFromEntityPos(pos));
        _players.Add(player);
        player.tangibleObject = Instantiate(Resources.Load("Prefab/Player") as GameObject, new Vector3(pos.x, pos.y, -10), Quaternion.identity);
        player.tangibleObject.name = "Pawn" + id;
        GameObject playerInfo = Instantiate(Resources.Load("Prefab/PlayerInfo") as GameObject, new Vector3(pos.x, pos.y, -10), Quaternion.identity);
        HealthHandler healthHandler = playerInfo.AddComponent<HealthHandler>();
        healthHandler.Initialize(player,true);
        AddButtonTo(player);

        GameObject helperConnection = Instantiate(Resources.Load("Prefab/textID") as GameObject,new Vector3(-20,0,-5), Quaternion.identity);
        helperConnection.transform.SetParent(player.tangibleObject.transform);
        helperConnection.name = "helper" + player.globalId;
        helperConnection.GetComponent<TextMeshPro>().text = player.globalId;
        player.helpConnection = helperConnection;
    }

    public void CreateNewNpc(int id, string name) {
        Debug.Log("Creating NPC " + name + " (" + id + ")");
        Npc npc = new Npc(id.ToString(), name);
        _npcs.Add(npc);

        GameState gameState = GameObject.Find("TableQuests").GetComponent<GameState>();
        gameState._previousState = gameState._state;
        gameState._state = STATE.NEW_NPC;
    }

    public async void PlaceNewNpc(string tangibleId, Vector2 tangiblePosition) {
        Npc newNpc = _npcs[_npcs.Count-1];
        newNpc.updatePawnCode(tangibleId);
        newNpc.tilePosition = _grid.GetPosFromEntityPos(tangiblePosition);
        newNpc.tangibleObject = Instantiate(Resources.Load("Prefab/Monster") as GameObject, new Vector3(tangiblePosition.x, tangiblePosition.y, -10), Quaternion.identity);

        GameObject playerInfo = Instantiate(Resources.Load("Prefab/PlayerInfo") as GameObject, new Vector3(tangiblePosition.x, tangiblePosition.y, -10), Quaternion.identity);
        HealthHandler healthHandler = playerInfo.AddComponent<HealthHandler>();
        healthHandler.Initialize(newNpc, false);
        AddButtonTo(newNpc);
        if (newNpc.name == "Ogre") {
            newNpc.tangibleObject.transform.Find("Background").transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Ogre");
        }

        SocketIO client = GameObject.Find("TableQuests").GetComponent<InitializationSocket>()._client;
        Debug.Log("Tangible ID " + tangibleId);
        Debug.Log("New NPC final: name: " + newNpc.name + ", id: " + newNpc.id + ", pawnId: " + newNpc.pawnCode);
        await client.EmitAsync("newNpc", newNpc.pawnCode);
        
        GameState gameState = GameObject.Find("TableQuests").GetComponent<GameState>();
        gameState._state = gameState._previousState;
    }



    public void AddButtonTo(Entity entity) {
        var button = Instantiate(Resources.Load("Prefab/Button") as GameObject, new Vector3(), Quaternion.identity);
        button.transform.SetParent(entity.tangibleObject.transform);
        button.transform.localPosition = new Vector3(0, 1.4f, 0);
        button.transform.localScale = new Vector3(1, 1, 1);
        button.name = "buttonConfirm";
        button.SetActive(false);
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


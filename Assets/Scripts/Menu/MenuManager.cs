using Newtonsoft.Json;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
	List<Menu> menuList;
    Dictionary<string, GameObject> zoneInitDict;

    GridManager _grid;

    private float WIDTH_GRID_UNIT = 1/24f; // we're dividing the screen in a grid that is 24 tiles wide
    private float HEIGHT_GRID_UNIT = 1/15f; //same but 14 tiles high

	void Start()
	{
		menuList = new List<Menu>();
        zoneInitDict = new Dictionary<string, GameObject>();
        _grid = GameObject.Find("GridManager").GetComponent<GridManager>();
    }

    public bool Exists(string id)
    {
        return menuList.Where(p => p.id == id).Count() > 0;
    }

    public void Move(string id, Vector2 pos)
    {
        pos = GetCanvasPosition(pos);
        GetMenuWithId(id).Move(pos);
    }

    public void HandleNotOnTable(string id)
    {
        zoneInitDict.Remove(id);
        GameObject.Destroy(GetMenuWithId(id).tangibleObject);
        menuList.Remove(GetMenuWithId(id));
     
    }

    public void Rotate(string id, float degree)
    {
        GetMenuWithId(id).Rotate(degree);
    }   

    public Vector2 GetCanvasPosition(Vector2 pos)
    {
        float xCoord = pos.x / WIDTH_GRID_UNIT;
        float yCoord = -(pos.y / HEIGHT_GRID_UNIT) + 15;
        pos.x = _grid.GetTileAtPosition(0, 0).GetWidth() * xCoord;
        pos.y = _grid.GetTileAtPosition(0, 0).GetHeight() * yCoord;
        return pos; 
    }

    public void CreateNewMenu(string id, Vector2 pos)
    {
        Menu _menu = new Menu(id);
        menuList.Add(_menu);
        _menu.InstantiateMenu(pos);
        pos = GetCanvasPosition(pos);
        GameObject _zone = Instantiate(Resources.Load("Prefab/ZoneMenu") as GameObject, new Vector3(100, 50, -1), Quaternion.identity);
        _zone.transform.parent = _menu.tangibleObject.transform;
        _zone.name = "zone" + id;
        zoneInitDict.Add(id, _zone);
    }

    internal void SetMenuGlobalID(string idMenu, string idPlayer)
    {
       GetMenuWithId(idMenu).globalId = idMenu + idPlayer;
    }

    public string IsInZone(Vector2 positionTangible)
    {
        positionTangible = GetCanvasPosition(positionTangible);
        foreach (var item in zoneInitDict)
        {
            if (Vector2.Distance(positionTangible, item.Value.GetComponent<SpriteRenderer>().bounds.center) <= item.Value.GetComponent<SpriteRenderer>().bounds.extents.x)
            {
                Destroy(GameObject.Find(item.Value.name)); ;
                string value = item.Value.name.Replace("zone", "");
                zoneInitDict.Remove(value);
                return value;
            }
        }
        return "";
    }

    public Menu GetMenuWithId(string id) {
        Predicate<Menu> matchingId = delegate(Menu currentMenu) { return currentMenu.id == id; };
        return menuList.Find(matchingId);
    }

    public void populateMenu()
    {
        List<Player> players = GameObject.Find("TableQuests").GetComponent<GameState>()._entityManager._players;
        foreach (Menu menu in menuList)
        {
            int i = 0;
            foreach (Player player in players)
            {
                GameObject button = Instantiate(Resources.Load("Prefab/Button") as GameObject, new Vector3(), Quaternion.identity);
                button.transform.SetParent(menu.tangibleObject.transform);
               // button.transform.localPosition = new Vector3(0, 1.4f, 0);
                float angle = i * Mathf.PI * 2f / (float)players.Count();
                float radius = 1.5f;
                button.transform.localPosition = new Vector3(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius, 0);
                button.transform.localScale = new Vector3(1, 1, 1);
                button.transform.GetComponent<OnClickButton>().call = delegate { attackPlayerButtonClick(player.globalId); };
                i++;
            }
            GameObject helperConnection = Instantiate(Resources.Load("Prefab/textID") as GameObject, new Vector3(0, 0 - 5), Quaternion.identity);
            helperConnection.transform.SetParent(menu.tangibleObject.transform);
            helperConnection.name = "helper" + menu.globalId;
            helperConnection.GetComponent<TextMeshPro>().text = menu.globalId;
            helperConnection.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    public async void attackPlayerButtonClick(string globalIDPlayer)
    {
        var myData = new
        {
            id = globalIDPlayer,
            life = "50",
        };
        Debug.Log("Removing life to " + globalIDPlayer);
        //Tranform it to Json object
        string jsonData = JsonConvert.SerializeObject(myData);
        SocketIO client = GameObject.Find("TableQuests").GetComponent<InitializationSocket>()._client;
        await client.EmitAsync("attackPlayer", jsonData);
    }

    public bool hasPlayer(string idMenu)
    {
        return GetMenuWithId(idMenu).globalId != null;
    }


}


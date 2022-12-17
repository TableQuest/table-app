using Newtonsoft.Json;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
	public List<Menu> menuList;
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
        pos.x = Tile.WIDTH * xCoord;
        pos.y = Tile.HEIGHT * yCoord;
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
        foreach(Menu menu in menuList)
        {
            if (menu.globalId == id || menu.id == id)
            {
                return menu;
            } 
        }
        return null;
       // Predicate<Menu> matchingId = delegate(Menu currentMenu) { return currentMenu.id == id; };
       // return menuList.Find(matchingId);
    }

    public void populateMenu()
    {
        foreach (Menu menu in menuList)
        {
            menu.listPagesButton = MenuBuilder.generatePages(menu.globalId);
            MenuBuilder.InstantiateButton(menu);
            MenuBuilder.DisplayPage(0,menu);
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


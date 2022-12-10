using System;
using System.Collections.Generic;
using System.Linq;
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
        float xCoord = pos.x / WIDTH_GRID_UNIT;
        float yCoord = -(pos.y / HEIGHT_GRID_UNIT) + 15 ;
        pos.x = _grid.GetTileAtPosition(0, 0).GetWidth() * xCoord;
        pos.y = _grid.GetTileAtPosition(0, 0).GetHeight() * yCoord;
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

    public void CreateNewMenu(string id, Vector2 pos)
    {
        Menu _menu = new Menu(id);
        menuList.Add(_menu);
        _menu.InstantiateMenu(pos);
        float xCoord = pos.x / WIDTH_GRID_UNIT;
        float yCoord = -(pos.y / HEIGHT_GRID_UNIT) + 15;
        pos.x = _grid.GetTileAtPosition(0, 0).GetWidth() * xCoord;
        pos.y = _grid.GetTileAtPosition(0, 0).GetHeight() * yCoord;
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
        foreach (var item in zoneInitDict)
        {
            float xCoord = positionTangible.x / WIDTH_GRID_UNIT;
            float yCoord = -(positionTangible.y / HEIGHT_GRID_UNIT) + 15;
            positionTangible.x = _grid.GetTileAtPosition(0, 0).GetWidth() * xCoord;
            positionTangible.y = _grid.GetTileAtPosition(0, 0).GetHeight() * yCoord;
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
}


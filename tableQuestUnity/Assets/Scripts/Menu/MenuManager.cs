using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
	List<Menu> menuList;
    List<GameObject> zoneInitList;

    GridManager _grid;

    private float WIDTH_GRID_UNIT = 1/24f; // we're dividing the screen in a grid that is 24 tiles wide
    private float HEIGHT_GRID_UNIT = 1/15f; //same but 14 tiles high

	void Start()
	{
		menuList = new List<Menu>();
        zoneInitList = new List<GameObject>();        
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

    public void CreateNewMenu(string id, Vector2 pos)
    {
        Menu _menu = new Menu(id);
        menuList.Add(_menu);
        _menu.InstantiateMenu(pos);
        GameObject _zone = Instantiate(Resources.Load("Prefab/ZoneMenu") as GameObject, new Vector3(pos.x + 50, pos.y, -10), Quaternion.identity);
        _zone.name = "zone" + id;
        zoneInitList.Add(_zone);
    }

    internal void SetMenuGlobalID(string idMenu, string idPlayer)
    {
       GetMenuWithId(idMenu).globalId = idMenu + idPlayer;
    }

    public string IsInZone(Vector2 positionTangible)
    {
        foreach (GameObject zone in zoneInitList)
        {
            if (Vector2.Distance(positionTangible, zone.GetComponent<SpriteRenderer>().bounds.center) <= zone.GetComponent<SpriteRenderer>().bounds.extents.x)
            {
                Destroy(GameObject.Find(zone.name)); ;
                string value = zone.name.Replace("zone", "");
                zoneInitList.Remove(zone);
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


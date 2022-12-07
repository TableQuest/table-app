using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
	List<Menu> menuList;
    List<GameObject> zoneInitList;


	public MenuManager()
	{
		this.menuList = new List<Menu>();
        this.zoneInitList = new List<GameObject>();
    }

    public bool Exists(string id)
    {
        foreach (Menu menu in menuList)
        {
            if (menu.id == id)
            {
                return true;
            }
        }
        return false;
    }

    public void Move(string id, Vector2 pos)
    {
        foreach(Menu menu in menuList)
        {
            if(menu.id == id)
            {
                menu.Move(pos);
            }
        }
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
       foreach(Menu menu in menuList)
        {
            if(menu.id == idMenu)
            {
                menu.globalId = idMenu + idPlayer;
            }
        }
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
}


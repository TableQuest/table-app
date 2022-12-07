using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager
{
	List<Menu> menuList;
    List<GameObject> zoneInitList;

	public MenuManager()
	{
		this.menuList = new List<Menu>();
        this.zoneInitList = new List<GameObject>();
	}

    internal bool Exists(string id)
    {
        throw new NotImplementedException();
    }

    internal void Move(string id, Vector2 pos)
    {
        throw new NotImplementedException();
    }


}


using System;
using UnityEngine;

public enum STATE
{
	INIT,
	PLAYING
}

public class GameState
{
	public EntityManager _entityManager;
	public MenuManager _menuManager;

	public STATE _state;

	public GameState()
	{
		_entityManager = new EntityManager();
		_menuManager = new MenuManager();
		_state = STATE.INIT;
	}

	public void HandleTangibleEvents(string id, Vector2 pos)
	{
		switch(_state)
		{
			case STATE.INIT:
				HandleEventInit(id, pos);
				break;
			case STATE.PLAYING:
				MoveTangiblePlaying(id, pos);
				break;
			default:
				break;
		}
	}

    private void HandleEventInit(string id, Vector2 pos)
	{
		if (!_entityManager.Exists(id) && !_menuManager.Exists(id))
		{
			if (!IsInZone(id))
			{
				_menuManager.createZone();
			}
		}
	}
    

	private void MoveTangiblePlaying(string id, Vector2 pos)
	{
		if (_entityManager.Exists(id))
		{
			_entityManager.Move(id, pos);
		}
		else if (_menuManager.Exists(id))
		{
			_menuManager.Move(id, pos);
		}
	}
}


using System;
using UnityEngine;

public enum STATE
{
	INIT,
	PLAYING
}

public class GameState : MonoBehaviour
{
	public EntityManager _entityManager;
	public MenuManager _menuManager;

	public STATE _state;

	void Start()
	{
		_entityManager = new EntityManager();
		_menuManager = new MenuManager();
		_state = STATE.INIT;
		gameObject.AddComponent<InitializationSocket>();
		GameObject.Find("Manager").GetComponent<MessageManager>().gameState = this;
		DontDestroyOnLoad(gameObject);
	}

	public void HandleTangibleEvents(string id, Vector2 pos, float rotation)
	{
		Debug.Log("state : " + _state.ToString());
		switch(_state)
		{
			case STATE.INIT:
				HandleEventInit(id, pos);
				MoveTangiblePlaying(id, pos, rotation);
				break;
			case STATE.PLAYING:
				MoveTangiblePlaying(id, pos, rotation);
				break;
			default:
				break;
		}
	}

    private void HandleEventInit(string id, Vector2 pos)
	{
		if (!_entityManager.Exists(id) && !_menuManager.Exists(id))
		{
			string _idMenu = _menuManager.IsInZone(pos);
			if(_idMenu == "")
			{
				Debug.Log("salut");
				_menuManager.CreateNewMenu(id,pos);
			} else
            {
				_entityManager.CreateNewPlayer(id, pos, _idMenu);
				_menuManager.SetMenuGlobalID(_idMenu, id);
            }
		}
	}
    

	private void MoveTangiblePlaying(string id, Vector2 pos, float rotation)
	{
		if (_entityManager.Exists(id))
		{
			_entityManager.Move(id, pos);
			_entityManager.Rotate(id, rotation);
		}
		else if (_menuManager.Exists(id))
		{
			_menuManager.Move(id, pos);
		}
	}
	
}


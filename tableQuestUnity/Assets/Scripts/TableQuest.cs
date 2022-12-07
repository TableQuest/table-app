using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableQuest : MonoBehaviour
{
    public GameState _gameState;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        _gameState = new GameState();
        Debug.Log(GameObject.Find("Manager").GetComponent<MessageManager>());
        GameObject.Find("Manager").GetComponent<MessageManager>().gameState = _gameState;
    }
}

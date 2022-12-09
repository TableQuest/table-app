using System.Collections;
using UnityEngine;
using SocketIOClient;
using System.Threading;
using System.Collections.Concurrent;
using System;
using Unity.VisualScripting;

public class InitializationSocket : MonoBehaviour
{
    private SocketIO _client;
    private Socket socket;
    private GameState _gameState;
    
    void Start()
    {
        socket = GameObject.Find("SocketClient").GetComponent<Socket>();
        _client = socket.client;
        _gameState = GameObject.Find("TableQuests").GetComponent<GameState>();
        DefineCoroutines();
    }

    public void DefineCoroutines()
    {
        var thread = new Thread(RouteThread);
        thread.Start();

        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil((() => socket._mainThreadhActions.Count > 0));
            if (!socket._mainThreadhActions.TryDequeue(out var action))
            {
                Debug.LogError("Something Went Wrong ! ", this);
                yield break;
            }
            
            action?.Invoke();
        }
    }


    private void RouteThread()
    {
        while (_client == null)
        {
            _client = socket.client;
            Thread.Sleep(300);
        }
        
        _client.On("playerConnection", (data) =>
        {
            string str = data.GetValue<string>(0);
            
            socket._mainThreadhActions.Enqueue(() =>
            {
                _gameState._entityManager.RemoveHelper(str);
            });
        });
        
        _client.On("switchStatePlaying", (data) =>
        {
            string str = data.GetValue<string>(0);
            
            socket._mainThreadhActions.Enqueue(() =>
            {
                GameObject.Find("TableQuests").GetComponent<GameState>()._state = STATE.PLAYING;
                Debug.Log("changing : "+_gameState._state);
            });
        });
    }


}

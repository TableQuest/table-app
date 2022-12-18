using UnityEngine;
using SocketIOClient;
using System.Threading;


public class InitializationSocket : MonoBehaviour
{
    public SocketIO _client;
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
                _gameState._menuManager.populateMenu();
            });
        });
        
        _client.On("switchStateConstraint", data =>
        {
            string str = data.GetValue<string>(0);
            socket._mainThreadhActions.Enqueue(() =>
            {
                GameObject.Find("TableQuests").GetComponent<GameState>()._state = STATE.CONSTRAINT;
                Debug.Log("changing : "+_gameState._state);
            });
        });
    }


}

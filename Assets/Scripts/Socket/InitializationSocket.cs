using UnityEngine;
using SocketIOClient;
using System.Threading;
using Newtonsoft.Json;

public class InitializationSocket : MonoBehaviour
{
    public SocketIO _client;
    private Socket socket;
    private GameState _gameState;
    bool firstSwitch = true;
    
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
        

        _client.On("switchState", (data) =>
        {
            string str = data.GetValue<string>(0);
            switch(str) {
                case "FREE":
                    socket._mainThreadhActions.Enqueue(() =>
                    {
                        _gameState._state = STATE.PLAYING;
                        Debug.Log("changing to: "+_gameState._state);
                        if(firstSwitch) {
                            _gameState._menuManager.populateMenu();
                            firstSwitch = false;
                        }
                    });
                    break;

                case "RESTRICTED":
                    socket._mainThreadhActions.Enqueue(() =>
                    {
                        _gameState._state = STATE.CONSTRAINT;
                        Debug.Log("changing to: "+_gameState._state);
                    });
                    break;
                
                default: Debug.Log("State " + str + " is wrong or not implemented yet.");
                    break;
            }
        });

        _client.On("newNpc", (data) =>
        {
            socket._mainThreadhActions.Enqueue(() =>
            {
                Debug.Log("Getting NPC data: " + data);
                string str = data.GetValue<string>(0);
                TempNpc npcData = JsonConvert.DeserializeObject<TempNpc>(str);
                _gameState._entityManager.CreateNewNpc(npcData.id, npcData.name); //normalement cet ID c'est celui du monstre (10: Goblin, 11: Ogre)
            });
        });
    }

}

public class TempNpc {
    public string description;
    public int id;
    public int life;
    public int lifeMax;
    public string name;
}

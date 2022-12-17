using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private GridManager _grid;
    private GameState _gameState;
    private Socket _socket;
    
    void Start()
    {
        _grid = GameObject.Find("GridManager").GetComponent<GridManager>();
        _gameState = GameObject.Find("TableQuests").GetComponent<GameState>();
        _socket = GameObject.Find("SocketClient").GetComponent<Socket>();
        DefineRoute();
    }

    private void DefineRoute()
    {
        _socket.client.On("playerMove", (response) =>
        {
            var str = response.GetValue<string>(0);
            
            _socket._mainThreadhActions.Enqueue(() =>
            {
                Debug.Log("Json receive for move : "+str);
                
                var playerMove = JsonUtility.FromJson<PlayerMove>(str);
                
                Debug.Log("Player move " + playerMove.playerId );
                
                var player = _gameState._entityManager.GetPlayerWithGlobalId(playerMove.playerId);
                Debug.Log("Player found : "+player.globalId);
                Debug.Log("player tangible : "+ player.tangibleObject.tag);
                var canvasPos = _grid.GetCanvasPosition(player.tangibleObject.transform.position);
                Debug.Log("canvasPos : "+canvasPos);
                // var gridPos = _grid.GetTileFromEntityPos(player.tangibleObject.transform.position);
                // var gridPos1 = _grid.GetTileFromEntityPos(canvasPos);
                // Debug.Log("Grid : " + gridPos.gameObject.name);
            });
            
        });
    }
}

class PlayerMove
{
    public string playerId;
    public int speed;
}


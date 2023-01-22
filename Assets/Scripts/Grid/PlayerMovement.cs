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
    public Player currPlayer;
    public Movement CurMovement;
    
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
                _grid.resetTilesAttack();
                var playerMove = JsonUtility.FromJson<PlayerMove>(str);
                var player = _gameState._entityManager.GetPlayerWithGlobalId(playerMove.playerId);
                
                // If there is not current Move yet.
                if (CurMovement == null && player != null) 
                {
                    var tilePos = _grid.GetPosFromEntityPos(player.tangibleObject.transform.position);
                    currPlayer = player;
                    var tiles = _grid.GetTilesAroundPosition(tilePos, playerMove.speed);
                    CurMovement = new(tiles, player, tilePos);
                    CurMovement.HighlightMove();
                }
                // If the current movement is not initialized yet. The movement can be replaced.
                else if (CurMovement != null && player == CurMovement.Player && player != null
                         && !CurMovement.IsMoving) 
                {
                    CurMovement.DeactivateMove();
                    CurMovement = null;
                }
                // If the current movement is from an other player, the current move is replace by the new one.
                else if (CurMovement != null && player != CurMovement.Player && player != null && !CurMovement.IsMoving) 
                {
                    var tilePos = _grid.GetPosFromEntityPos(player.tangibleObject.transform.position);
                    currPlayer = player;
                    var tiles = _grid.GetTilesAroundPosition(tilePos, playerMove.speed);
                    CurMovement.DeactivateMove();
                    CurMovement = new Movement(tiles, player, tilePos);
                    CurMovement.HighlightMove();
                }
            });
            
        });
    }

    // Replace the player when a wrong move has been registered. If all players are replaced correctly, it removes the WRONG state.
    public void ReplacePlayer(Player player, Vector2 oscPos, float rotation)
    {
        var tilePos = _grid.GetPosFromEntityPos(_grid.GetCanvasPosition(oscPos));
        if (tilePos.Equals(player.tilePosition))
        {
            Debug.Log("BONNE POSITION !");
            player.postionCheck = true;
        }
        else
        {
            player.postionCheck = false;
        }
        
        var playerPosCheck = true;
        foreach (var p in _gameState._entityManager._players)
        {
            if (!p.postionCheck) playerPosCheck = false;
        }

        if (player.postionCheck && playerPosCheck)
        {
            Debug.Log("PLUS CONTRAINT");
            _gameState._state = STATE.CONSTRAINT;
            _gameState.WrongMove.SetActive(false);
        }
    }
    
    
    // Move a player that can move, and check if he is in the zone, outside or at initial position.
    public void MovePlayer(Player player, Vector2 oscPos, float rotation)
    {
        var tilePos = _grid.GetPosFromEntityPos(_grid.GetCanvasPosition(oscPos));
        
        // If the Current Movement is in the zone, move the tangible.
        if (CurMovement.IsInMoveZone(tilePos))
        {
            CurMovement.IsMoving = true;
            _gameState._entityManager.Move(player.id, oscPos);
            _gameState._entityManager.Rotate(player.id, rotation);
            ActivateMoveButton(player);
        }
        // If the current movement is at the initial position, move tangible and set "IsMoving" to false.
        else if (CurMovement.IsInInitialPosition(tilePos))
        {
            CurMovement.IsMoving = false;
            _gameState._entityManager.Move(player.id, oscPos);
            _gameState._entityManager.Rotate(player.id, rotation);
            DeactivateMoveButton(player);
        }
        // If the current Movement is not in the zone. The state is WRONG. The player must replace his pawn.
        else
        {
            _gameState._state = STATE.WRONG;
            _gameState.WrongMove.SetActive(true);
            DeactivateMoveButton(player);
        }
    }
    
    // When a player validate the move in constraint mode. it deactivates the highlighted tiles and confirms the move.
    private void ValidateMove()
    {
        DeactivateMoveButton(CurMovement.Player);
        CurMovement.DeactivateMove();
        CurMovement = null;
    }

    public bool TilePositionChanged(Player player, Vector2 oscPos)
    {
        var canvasPos = _grid.GetCanvasPosition(oscPos);
        var tilePos = _grid.GetPosFromEntityPos(canvasPos);
        
        return !tilePos.Equals(player.tilePosition);
    }
    
    public void ActivateMoveButton(Player player)
    {
        var button = player.tangibleObject.transform.GetChild(0);
        button.gameObject.SetActive(true);
        button.GetComponent<OnClickButton>().call = ValidateMove;
    }

    public void DeactivateMoveButton(Player player)
    {
        var button = player.tangibleObject.transform.GetChild(0);
        button.gameObject.SetActive(false);
        button.GetComponent<OnClickButton>().call = null;
    }
}

class PlayerMove
{
    public string playerId;
    public int speed;
}


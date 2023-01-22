using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// thanks Tarodev for this awesome tutorial https://www.youtube.com/watch?v=kkAjpQAM-jE
public class GridManager : MonoBehaviour
{
    [SerializeField] private int nbTilesWidth;
    [SerializeField] private int nbTilesHeight;
    
    private float WIDTH_GRID_UNIT = 1/24f; // we're dividing the screen in a grid that is 25 tiles wide
    private float HEIGHT_GRID_UNIT = 1/15f; //same but 14 tiles high

    [SerializeField] private Tile tile;
    [SerializeField] private GameObject gridObject;
    public List<Tile> tilesAttack = new List<Tile>();
    public string globalIDPlayerAttack;
    public int currentSkillId = -1;
    private Dictionary<Vector2, Tile> tiles;
    private Dictionary<Vector2, Tile> tilesInit;
    public GameState _gameState;

    void Start() {
        GenerateGrid();
        _gameState = GameObject.Find("TableQuests").GetComponent<GameState>();
        GameObject gridObject = GameObject.Find("Grid");
        gridObject.transform.position = new Vector3(gridObject.transform.position.x, gridObject.transform.position.y, 4);
    }

    void GenerateGrid() {
        tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < nbTilesWidth; x++) {
            for (int y = 0; y < nbTilesHeight; y++)
            {
                if (x == 0 || x == nbTilesWidth - 1 || y == 0 || y == nbTilesHeight - 1 || y == nbTilesHeight - 2 ||
                    (y == 5 && x >= 1 && x <= 4) ||
                    ((y == 4 || y == 3) && x == 4) ||
                    (x == 9 && (y >= 1 && y <= 9)) ||
                    (x == 8 && (y >= 9 && y <=12)))    
                {
                    tiles[new Vector2(x, y)] = null;
                }
                else
                {
                    var spawnedTile = Instantiate(tile,
                                                    new Vector3(x * Tile.WIDTH + Tile.WIDTH / 2,
                                                                y * Tile.HEIGHT + Tile.HEIGHT / 2),
                                                    Quaternion.identity, gridObject.transform);
                    spawnedTile.tilePos = new Vector2(x, y);
                    spawnedTile.name = $"Tile {x} {y}";

                    var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                    spawnedTile.Init(isOffset);

                    tiles[new Vector2(x, y)] = spawnedTile;
                }
            }
        }
    }
    
    /**
     * Get the Tile at the entity's position.
     */
    public Vector2 GetPosFromEntityPos(Vector2 canvasPosition)
    {
        var xCoord = (int)(canvasPosition.x / Tile.WIDTH);
        var yCoord = (int)(canvasPosition.y / Tile.HEIGHT);
        
        return new Vector2(xCoord, yCoord);
    }

    public void resetTilesAttack()
    {
        foreach (var tile in tilesAttack)
        {
            tile.PaintBaseColor();
        }
        tilesAttack = new List<Tile>();
    }

    public void resetTiles()
    {
        foreach (var item in tiles)
        {
            if(item.Value != null)
            {
                item.Value.PaintBaseColor();
            }
        }
    }
    
    public Vector2 GetCanvasPosition(Vector2 oscPos)
    {
        int xCoord = (int)(oscPos.x / WIDTH_GRID_UNIT);
        int yCoord = -(int)(oscPos.y / HEIGHT_GRID_UNIT) + 14 ;

        float xPosition = Tile.WIDTH * xCoord + Tile.WIDTH / 2;
        float yPosition = Tile.HEIGHT * yCoord + Tile.HEIGHT / 2;
        
        return new Vector2(xPosition, yPosition);
    }

    
    public Tile GetTileAtPosition(Vector2 tilePos) {
        if (tiles.TryGetValue(tilePos, out var tile))
        {
            return tile;
        }
        return null;
    }

    public Tile GetTileAtPosition(int x, int y) {
        return GetTileAtPosition(new Vector2(x, y));
    }

    public List<Tile> GetTilesAroundPosition(Vector2 tilePos, int radius)
    {
        List<Tile> list = new();
        Debug.Log($"Around pos {tilePos.ToString()} with a radius of {radius}");
        List<Player> players = _gameState._entityManager._players;
        for (int x = (int)tilePos.x-radius; x <= tilePos.x+radius; x++)
        {
            for (int y = (int)tilePos.y-radius; y <= tilePos.y + radius; y++)
            {
                if (x >= 0 && x < nbTilesWidth && y >= 0 && y < nbTilesHeight && !(x == (int)tilePos.x && y == (int)tilePos.y))
                {
                    Tile tile = GetTileAtPosition(x, y);
                    if (tile != null)
                    {
                        list.Add(tile);
                    }
                }
            }
        }
        
        return list;
    }

    public int GetWidth() {
        return nbTilesWidth;
    }

    public int GetHeight() {
        return nbTilesHeight;
    }
}

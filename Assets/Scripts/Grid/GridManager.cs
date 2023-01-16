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
    private Dictionary<Vector2, Tile> tiles;

    void Start() {
        GenerateGrid();
    }

    void GenerateGrid() {
        tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < nbTilesWidth; x++) {
            for (int y = 0; y < nbTilesHeight; y++)
            {
                var spawnedTile = Instantiate(tile,
                                                new Vector3(x*Tile.WIDTH+Tile.WIDTH/2,
                                                            y*Tile.HEIGHT+Tile.HEIGHT/2),
                                                Quaternion.identity, gridObject.transform);
                spawnedTile.tilePos = new Vector2(x, y);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);
                
                tiles[new Vector2(x, y)] = spawnedTile;
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

        for (int x = (int)tilePos.x-radius; x <= tilePos.x+radius; x++)
        {
            for (int y = (int)tilePos.y-radius; y <= tilePos.y + radius; y++)
            {
                if (x >= 0 && x < nbTilesWidth && y >= 0 && y < nbTilesHeight && !(x == (int)tilePos.x && y == (int)tilePos.y))
                {
                    list.Add(GetTileAtPosition(x, y));
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// thanks Tarodev for this awesome tutorial https://www.youtube.com/watch?v=kkAjpQAM-jE
public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, height;

    [SerializeField] private Tile tile;

    [SerializeField] private Transform canvas;

    private Dictionary<Vector2, Tile> tiles;

    void Start() {
        GenerateGrid();
    }

    void GenerateGrid() {
        tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++)
            {
                var spawnedTile = Instantiate(tile,
                                                new Vector3(x*tile.GetWidth()+tile.GetWidth()/2,
                                                            y*tile.GetHeight()+tile.GetHeight()/2),
                                                Quaternion.identity, canvas);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);
                
                tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
    }

    public Vector2 GetCanvasPosition(Vector2 pos)
    {
        float xCoord = (int)(pos.x / (1/width));
        float yCoord = -(int)(pos.y / (1/height)) + 14 ;
        
        Debug.Log("pos in func "+xCoord + " "+ yCoord);
        float xPosition = GetTileAtPosition(0, 0).GetWidth() * xCoord + GetTileAtPosition(0, 0).GetWidth() / 2;
        float yPosition = GetTileAtPosition(0, 0).GetHeight() * yCoord + GetTileAtPosition(0, 0).GetHeight() / 2;
        return new Vector2(xPosition, yPosition);
    }

    public Tile GetTileFromEntityPos(Vector2 pos)
    {
        var xCoord = (int)(pos.x / (1/width));
        var yCoord = -(int)(pos.y / (1/height)) + 14 ;
        
        Debug.Log("x :" + xCoord);
        Debug.Log("y :" + yCoord);
        return null;
    }
    
    public Tile GetTileAtPosition(Vector2 pos) {
        Debug.Log("trying find with vector : " + pos);
        if (tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        Debug.Log("None found");
        return null;
    }

    public Tile GetTileAtPosition(int x, int y) {
        return GetTileAtPosition(new Vector2(x, y));
    }

    public int GetWidth() {
        return width;
    }

    public int GetHeight() {
        return height;
    }
}

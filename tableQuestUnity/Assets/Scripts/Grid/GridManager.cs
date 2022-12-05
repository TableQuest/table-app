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
                                                new Vector3(x*tile.getWidth()+tile.getWidth()/2,
                                                            y*tile.getHeight()+tile.getHeight()/2),
                                                Quaternion.identity, canvas);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);
                
                tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
    }

    public Tile getTileAtPosition(Vector2 pos) {
        if (tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }

    public Tile getTileAtPosition(int x, int y) {
        return getTileAtPosition(new Vector2(x, y));
    }
}

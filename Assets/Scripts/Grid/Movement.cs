using System.Collections.Generic;
using UnityEngine;

public class Movement
{
    public Player Player;
    
    public List<Tile> _tiles;
    private Vector2 _initialTilePosition;
    public bool IsMoving = false; // use to check if a player already move from the initial position but didn't validate the move yet.
    
    public Movement(List<Tile> tiles, Player player, Vector2 initialTilePosition)
    {
        Player = player;
        _tiles = tiles;
        _initialTilePosition = initialTilePosition;
    }


    public bool IsInMoveZone(Vector2 tilePos)
    {
        foreach (var tile in _tiles)
        {
            if (tile.tilePos.Equals(tilePos)) return true;
        }
        return false;
    }

    public void HighlightMove()
    {
        foreach (var tile in _tiles)
        {
            tile.Highlight(Color.white);
        }
    }

    public void DeactivateMove()
    {
        foreach (var tile in _tiles)
        {
            tile.PaintBaseColor();
        }
    }

    public bool IsInInitialPosition(Vector2 tilePos)
    {
        return tilePos.Equals(_initialTilePosition);
    }
}
using UnityEngine;

public class Position
{
    private float WIDTH_GRID_UNIT = 0.04f; // we're dividing the screen in a grid that is 25 tiles wide
    private float HEIGHT_GRID_UNIT = 1.125f/14f; //same but 14 tiles high
    private float XCoord;
    private float YCoord;
    public Vector2 TUIOPosition
    {
        get => new Vector2(XCoord, YCoord);
        set
        {
            XCoord = (int)(value.x / WIDTH_GRID_UNIT) * WIDTH_GRID_UNIT + WIDTH_GRID_UNIT/2;
            YCoord = (int)(value.y * 1.125f / HEIGHT_GRID_UNIT) * HEIGHT_GRID_UNIT;
        }
    }
    public Position(float x, float y)
    {
        XCoord = (x / WIDTH_GRID_UNIT) * WIDTH_GRID_UNIT + WIDTH_GRID_UNIT/2;
        YCoord = (y * 1.125f / HEIGHT_GRID_UNIT) * HEIGHT_GRID_UNIT;
    }
    public override string ToString()
    {
        return $"Position({XCoord},{YCoord})"; ;
    }
}

using UnityEngine;

public class Position
{
    private float XCoord;
    private float YCoord;
    public Vector2 TUIOPosition
    {
        get => new Vector2(XCoord, YCoord);
        set
        {
            XCoord = value.x;
            YCoord = value.y;
        }
    }
    public Position(float x, float y)
    {
        XCoord = x;
        YCoord = y;
    }
    public override string ToString()
    {
        return $"Position({XCoord},{YCoord})"; ;
    }
}

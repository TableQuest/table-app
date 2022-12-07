using System;
using UnityEngine;

public class TuioObject : TuioEntity
{
    public readonly string value;
    public TuioObject(int id, float x, float y, string value) : base(id, x, y, value)
    {
        this.value = value;
    }

    public string GetValue()
    {
        return value;
    }

    public bool IsOnTable()
    {
        return state != TuioState.CLICK_UP;
    }
    public override string ToString()
    {
        return $"Id : {Id} onTable: {IsOnTable()} drag: {IsDrag()}  {position} value: {value}\n";
    }


}
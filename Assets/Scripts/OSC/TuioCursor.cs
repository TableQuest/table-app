

public class TuioCursor : TuioEntity
{
    public TuioCursor(int id, float x, float y) : base(id, x, y,"")
    {
    }

    public bool IsClick()
    {
        return previousState == TuioState.MAINTAIN_DOWN && state == TuioState.CLICK_UP;
    }

    public bool IsLongClick()
    {
        return previousState == TuioState.LONG_CLICK && state == TuioState.CLICK_UP;
    }

    public override string ToString()
    {
        return $"detection numero :{Id} clic: {IsClick()} drag: {IsDrag()} longclic: {IsLongClick()} {position}\n";
    }
}

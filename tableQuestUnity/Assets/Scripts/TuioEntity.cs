using UnityEngine;

public abstract class TuioEntity
{
    protected TuioState previousState;
    protected TuioState state;
    public TuioState State
    {
        get => state;
        set
        {
            previousState = state;
            state = value;
        }
    }
    public int Id { get; private set; }
    public Position position;


    public TuioEntity(int id, float x, float y)
    {
        this.Id = id;
        position = new Position(x, y);
        state = TuioState.CLICK_DOWN;
        previousState = TuioState.CLICK_DOWN;
    }

    public bool IsDrag()
    {
        return state == TuioState.DRAG;
    }

    public void UpdateCoordinates(Vector2 newPosition)
    {
        //to trigger one time the event clickdown
        if (state == TuioState.CLICK_DOWN)
            State = TuioState.MAINTAIN_DOWN;

        if (state == TuioState.DRAG || Vector2.Distance(newPosition, position.TUIOPosition) > 0.01f)
        {
            State = TuioState.DRAG;
            position.TUIOPosition = newPosition;
        }
    }
}

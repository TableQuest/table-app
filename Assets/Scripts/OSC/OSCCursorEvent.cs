using UnityEngine;
using UnityEngine.Events;

class OSCCursorEvent : OSCEvent
{
    [SerializeField]
    public UnityEvent OnClick;
    [SerializeField]
    public UnityEvent OnLongClick;

    public override void RunFunction(TuioEntity tuio)
    {
        if (tuio is TuioCursor t)
        {
            if (t.IsDrag())
            {
                OnDrag.Invoke();
            }
            if (t.State == TuioState.CLICK_DOWN)
            {
                OnClickDown.Invoke();
            }
            if (t.State == TuioState.CLICK_UP)
            {
                OnClickUp.Invoke();
            }
            if (t.IsClick())
            {
                OnClick.Invoke();
            }
            if (t.IsLongClick())
            {
                OnLongClick.Invoke();
            }
            if (!detections.Contains(tuio))
            {
                OnCollisionEnter.Invoke("");
                detections.Add(tuio);
            }
        }
    }
}

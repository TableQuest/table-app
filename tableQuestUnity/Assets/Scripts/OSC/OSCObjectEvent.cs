using System.Collections.Generic;
using UnityEngine;

class OSCObjectEvent : OSCEvent
{

    public override void RunFunction(TuioEntity tuio)
    {

        if (tuio is TuioObject t)
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
            if (!detections.Contains(t))
            {
                OnCollisionEnter.Invoke(t.GetValue());
                detections.Add(t);
            }
        }
    }
}

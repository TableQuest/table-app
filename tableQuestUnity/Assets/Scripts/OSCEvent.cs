using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class OSCEvent : MonoBehaviour
{
    [SerializeField]
    protected UnityEvent OnClickDown;
    [SerializeField]
    protected UnityEvent OnClickUp;
    [SerializeField]
    protected UnityEvent OnDrag;
    [SerializeField]
    protected UnityEvent<string> OnCollisionEnter;
    [SerializeField]
    protected UnityEvent<string> OnCollisionExit;

    public List<TuioEntity> detections = new List<TuioEntity>();

    public abstract void RunFunction(TuioEntity tuio);

    private void Update()
    {
        List<TuioEntity> toRemove = new List<TuioEntity>();
        foreach (TuioEntity t in detections)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(t.position.TUIOPosition.x * Screen.width, t.position.TUIOPosition.y * Screen.height, 0));
            bool touched = Physics.Raycast(ray, out RaycastHit hit);
            if (t.State == TuioState.CLICK_UP || hit.transform != transform || !touched)
            {
                if (t is TuioObject tuio)
                {
                    OnCollisionExit.Invoke(tuio.GetValue());
                }
                else 
                {
                    OnCollisionExit.Invoke("");
                }
                toRemove.Add(t);
            }
        }
        foreach (TuioEntity t in toRemove)
            detections.Remove(t);
    }
}

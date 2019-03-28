using UnityEngine;

public abstract class BaseControl : MonoBehaviour {
    
    protected bool Pressed;
    protected Vector2 PressPosition;
    protected Vector2 Offset;

    public virtual void Select(Vector2 pressPosition)
    {
        Pressed = true;
        PressPosition = pressPosition;
    }

    public virtual void Reset()
    {
        Pressed = false;
        PressPosition = Vector2.zero;
    }

    public abstract void Submit();
    public abstract void Refresh();    
}

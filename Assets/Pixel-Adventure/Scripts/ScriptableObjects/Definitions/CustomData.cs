using UnityEngine;

public class CustomData : ScriptableObject
{
    public virtual void CaptureFrom(PlacedObject target) { }
    public virtual void ApplyTo(PlacedObject target)
    {
        target.customData = this;
    }
}

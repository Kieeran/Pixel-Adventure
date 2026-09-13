using UnityEngine;

public class CustomData : ScriptableObject
{
    // Save level
    public virtual void CaptureFrom(PlacedObject target) { }
    // Edit level
    public virtual void ApplyTo(PlacedObject target)
    {
        target.customData = this;
    }
}

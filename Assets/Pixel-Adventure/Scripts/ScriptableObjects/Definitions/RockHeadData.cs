using UnityEngine;

[CreateAssetMenu(fileName = "RockHeadData", menuName = "Scriptable Objects/RockHeadData")]
public class RockHeadData : CustomData
{
    public override void ApplyTo(PlacedObject target)
    {
        base.ApplyTo(target);
        if (target is not RockHead rockHead) return;
    }

    public override void CaptureFrom(PlacedObject target)
    {
        if (target is not RockHead rockHead) return;
    }
}

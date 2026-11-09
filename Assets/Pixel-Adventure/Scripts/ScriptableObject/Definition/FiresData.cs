using UnityEngine;

[CreateAssetMenu(fileName = "FiresData", menuName = "Scriptable Objects/FiresData")]
public class FiresData : CustomData
{
    public int count = 1;

    public override void ApplyTo(PlacedObject target)
    {
        base.ApplyTo(target);
        if (target is not Fires fires) return;

        fires.SetCount(count);
    }

    public override void CaptureFrom(PlacedObject target)
    {
        if (target is not Fires fires) return;

        count = fires.count;
    }
}

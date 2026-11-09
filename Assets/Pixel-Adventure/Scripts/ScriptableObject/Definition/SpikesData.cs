using UnityEngine;

[CreateAssetMenu(fileName = "SpikesData", menuName = "Scriptable Objects/SpikesData")]
public class SpikesData : CustomData
{
    public int count = 1;

    public override void ApplyTo(PlacedObject target)
    {
        base.ApplyTo(target);
        if (target is not Spikes spikes) return;

        spikes.SetCount(count);
    }

    public override void CaptureFrom(PlacedObject target)
    {
        if (target is not Spikes spikes) return;

        count = spikes.count;
    }
}

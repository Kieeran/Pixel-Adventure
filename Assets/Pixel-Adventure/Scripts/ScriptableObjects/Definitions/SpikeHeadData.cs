using UnityEngine;

[CreateAssetMenu(fileName = "SpikeHeadData", menuName = "Scriptable Objects/SpikeHeadData")]
public class SpikeHeadData : CustomData
{
    public override void ApplyTo(PlacedObject target)
    {
        base.ApplyTo(target);
        if (target is not SpikeHead spikeHead) return;
    }

    public override void CaptureFrom(PlacedObject target)
    {
        if (target is not SpikeHead spikeHead) return;
    }
}

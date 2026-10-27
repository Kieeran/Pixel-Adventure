using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpikeHeadData", menuName = "Scriptable Objects/SpikeHeadData")]
public class SpikeHeadData : CustomData
{
    public List<SpikeHead.MovementDirection> directions = new();
    [Min(0f)] public float delayTime;

    public override void ApplyTo(PlacedObject target)
    {
        base.ApplyTo(target);
        if (target is not SpikeHead spikeHead) return;

        spikeHead.SetDirections(directions);
        spikeHead.delayTime = delayTime;
    }

    public override void CaptureFrom(PlacedObject target)
    {
        if (target is not SpikeHead spikeHead) return;

        directions = new List<SpikeHead.MovementDirection>(spikeHead.directions);
        delayTime = spikeHead.delayTime;
    }
}

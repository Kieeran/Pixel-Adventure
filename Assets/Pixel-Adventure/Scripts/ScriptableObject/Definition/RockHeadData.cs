using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RockHeadData", menuName = "Scriptable Objects/RockHeadData")]
public class RockHeadData : CustomData
{
    public List<RockHead.MovementDirection> directions = new();
    [Min(0f)] public float delayTime;

    public override void ApplyTo(PlacedObject target)
    {
        base.ApplyTo(target);
        if (target is not RockHead rockHead) return;

        rockHead.SetDirections(directions);
        rockHead.delayTime = delayTime;
    }

    public override void CaptureFrom(PlacedObject target)
    {
        if (target is not RockHead rockHead) return;

        directions = new List<RockHead.MovementDirection>(rockHead.directions);
        delayTime = rockHead.delayTime;
    }
}

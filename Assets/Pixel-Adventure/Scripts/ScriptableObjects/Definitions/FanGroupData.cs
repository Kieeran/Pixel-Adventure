using UnityEngine;

[CreateAssetMenu(fileName = "FanGroupData", menuName = "Scriptable Objects/FanGroupData")]
public class FanGroupData : CustomData
{
    public string groupId;
    public Vector2 pushDirection = Vector2.zero;

    public Vector2 colliderOffset;
    public Vector2 colliderSize;

    public override void ApplyTo(PlacedObject target)
    {
        base.ApplyTo(target);
        if (target is not Fan fan) return;

        fan.pushDirection = pushDirection;
        fan.SetColliderShape(colliderSize, colliderOffset);
    }

    public override void CaptureFrom(PlacedObject target)
    {
        if (target is not Fan fan) return;

        colliderOffset = fan.col.offset;
        colliderSize = fan.col.size;
    }
}

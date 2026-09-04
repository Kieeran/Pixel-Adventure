using UnityEngine;

[CreateAssetMenu(fileName = "FanGroupData", menuName = "Scriptable Objects/FanGroupData")]
public class FanGroupData : CustomData
{
    public string groupId;
    public Vector2 pushDirection = Vector2.zero;

    public Vector2 colliderOffset;
    public Vector2 colliderSize;

    // Particle
    public float minLifeTime;
    public float maxLifeTime;
    public float angle;

    public override void ApplyTo(PlacedObject target)
    {
        base.ApplyTo(target);
        if (target is not Fan fan) return;

        fan.pushDirection = pushDirection;
        fan.SetColliderShape(colliderSize, colliderOffset);
        fan.SetParticleSystemProperties(minLifeTime, maxLifeTime, angle);
    }

    public override void CaptureFrom(PlacedObject target)
    {
        if (target is not Fan fan) return;

        colliderOffset = fan.col.offset;
        colliderSize = fan.col.size;

        minLifeTime = fan.ps.main.startLifetime.constantMin;
        maxLifeTime = fan.ps.main.startLifetime.constantMax;
        angle = fan.ps.shape.angle;
    }
}

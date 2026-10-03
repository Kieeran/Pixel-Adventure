using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SawWaypointData
{
    public Vector2 position;
    public float waitTime;
}

[CreateAssetMenu(fileName = "SawData", menuName = "Scriptable Objects/SawData")]
public class SawData : CustomData
{
    public List<SawWaypointData> waypoints;
    public bool isLoop;
    public bool renderChains;

    // Chains properties
    public List<Vector2> chainWaypoints;
    public Vector2 rootPos;
    public float spacing;

    public override void ApplyTo(PlacedObject target)
    {
        base.ApplyTo(target);
        if (target is not Saw saw) return;

        saw.isLoop = isLoop;
        saw.SetRenderChains(renderChains);
        saw.SetChainsProperties(chainWaypoints, rootPos, spacing);

        foreach (var wp in waypoints)
        {
            GameObject o = new()
            {
                tag = "EditorOnly"
            };
            o.transform.SetParent(target.transform);
            o.transform.position = new(wp.position.x, wp.position.y, 0);
            o.AddComponent<SawWaypoint>().waitTime = wp.waitTime;
        }
    }

    public override void CaptureFrom(PlacedObject target)
    {
        if (target is not Saw saw) return;

        isLoop = saw.isLoop;
        renderChains = saw.renderChains;

        waypoints.Clear();
        foreach (Transform tf in target.transform)
        {
            if (tf.gameObject.CompareTag("EditorOnly"))
            {
                // Waypoint chưa gắn SawWaypoint thì xem như không đợi
                float waitTime = tf.TryGetComponent<SawWaypoint>(out var sawWaypoint) ? sawWaypoint.waitTime : 0f;
                waypoints.Add(new SawWaypointData
                {
                    position = new Vector2(tf.position.x, tf.position.y),
                    waitTime = waitTime
                });
            }
        }

        chainWaypoints.Clear();
        chainWaypoints = new List<Vector2>(saw.chains.waypoints);
        rootPos = saw.chains.rootPos;
        spacing = saw.chains.spacing;
    }
}

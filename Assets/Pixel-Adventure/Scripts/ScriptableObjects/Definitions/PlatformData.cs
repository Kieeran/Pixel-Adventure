using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlatformData", menuName = "Scriptable Objects/PlatformData")]
public class PlatformData : CustomData
{
    public List<Vector2> waypoints;
    public bool isBrownPlatform;

    public override void ApplyTo(PlacedObject target)
    {
        base.ApplyTo(target);
        if (target is not Platform platform) return;

        platform.SetPlatformSkin(isBrownPlatform);

        foreach (var wp in waypoints)
        {
            GameObject o = new()
            {
                tag = "EditorOnly"
            };
            o.transform.SetParent(target.transform);
            o.transform.position = new(wp.x, wp.y, 0);
        }
    }

    public override void CaptureFrom(PlacedObject target)
    {
        if (target is not Platform platform) return;

        if (platform.brownSkin.gameObject.activeSelf == true && platform.greySkin.gameObject.activeSelf == false)
        {
            isBrownPlatform = true;
        }
        else
        {
            isBrownPlatform = false;
        }

        waypoints.Clear();
        foreach (Transform tf in target.transform)
        {
            if (tf.gameObject.CompareTag("EditorOnly"))
            {
                waypoints.Add(new Vector2(
                    tf.position.x,
                    tf.position.y
                ));
            }
        }
    }
}

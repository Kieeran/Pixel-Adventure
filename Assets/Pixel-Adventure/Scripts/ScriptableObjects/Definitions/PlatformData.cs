using UnityEngine;

[CreateAssetMenu(fileName = "PlatformData", menuName = "Scriptable Objects/PlatformData")]
public class PlatformData : CustomData
{
    public bool isBrownPlatform;

    public override void ApplyTo(PlacedObject target)
    {
        base.ApplyTo(target);
        if (target is not Platform platform) return;

        platform.SetPlatformSkin(isBrownPlatform);
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
    }
}

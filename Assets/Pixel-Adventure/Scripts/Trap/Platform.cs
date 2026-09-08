using UnityEngine;

public class Platform : PlacedObject
{
    public Transform brownSkin;
    public Transform greySkin;

    public void SetPlatformSkin(bool isBrownPlatform)
    {
        brownSkin.gameObject.SetActive(isBrownPlatform);
        greySkin.gameObject.SetActive(!isBrownPlatform);
    }

    public override void OnSpawn()
    {
        SetPlatformSkin((customData as PlatformData).isBrownPlatform);
    }

    public override void OnDespawn()
    {

    }
}

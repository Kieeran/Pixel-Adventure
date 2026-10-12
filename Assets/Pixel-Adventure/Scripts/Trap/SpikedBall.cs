using UnityEngine;

public class SpikedBall : PlacedObject
{
    [SerializeField] SpikedBallCollision spikedBallCollision;

    void OnValidate()
    {
        spikedBallCollision = GetComponent<SpikedBallCollision>();
    }

    public override void OnSpawn()
    {

    }

    public override void OnDespawn()
    {

    }
}

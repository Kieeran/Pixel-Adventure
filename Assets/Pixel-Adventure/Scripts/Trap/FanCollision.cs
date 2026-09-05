using UnityEngine;

public class FanCollision : MonoBehaviour
{
    [SerializeField] Fan fan;

    PlayerMovement cachedPlayerMovement;

    void OnValidate()
    {
        fan = GetComponent<Fan>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            cachedPlayerMovement = collision.GetComponent<PlayerMovement>();
            cachedPlayerMovement.SetExternalPush(fan.pushDirection, fan.GetPushPower());
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            cachedPlayerMovement.SetExternalPush(fan.pushDirection, fan.GetPushPower());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            cachedPlayerMovement.ClearExternalPush();
            cachedPlayerMovement = null;
        }
    }
}

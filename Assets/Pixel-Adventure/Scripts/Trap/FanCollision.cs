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
            Debug.Log($"Fan(location at {fan.transform.position}) + Character Enter");
            cachedPlayerMovement = collision.GetComponent<PlayerMovement>();
            cachedPlayerMovement.StartFanPush(fan.pushDirection);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            Debug.Log($"Fan(location at {fan.transform.position}) + Character Stay");
            cachedPlayerMovement.ApplyFanPush(fan.pushDirection, fan.GetPushPower());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            Debug.Log($"Fan(location at {fan.transform.position}) + Character Exit");
            cachedPlayerMovement.StopFanPush();
            cachedPlayerMovement = null;
        }
    }
}

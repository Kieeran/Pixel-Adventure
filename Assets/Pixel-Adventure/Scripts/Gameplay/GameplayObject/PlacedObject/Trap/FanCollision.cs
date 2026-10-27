using UnityEngine;

public class FanCollision : MonoBehaviour
{
    [SerializeField] Fan fan;

    PlayerPhysic cachedPlayerPhysic = null;

    void OnValidate()
    {
        fan = GetComponent<Fan>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            cachedPlayerPhysic = collision.GetComponentInParent<PlayerPhysic>();
            cachedPlayerPhysic.SetExternalForce(fan.pushDirection, fan.GetPushPower());
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            cachedPlayerPhysic.SetExternalForce(fan.pushDirection, fan.GetPushPower());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            cachedPlayerPhysic.ClearExternalForce();
            cachedPlayerPhysic = null;
        }
    }

    void OnDisable()
    {
        if (cachedPlayerPhysic != null)
        {
            cachedPlayerPhysic.ClearExternalForce();
            cachedPlayerPhysic = null;
        }
    }
}

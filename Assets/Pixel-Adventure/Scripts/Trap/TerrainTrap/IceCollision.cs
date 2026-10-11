using UnityEngine;

public class IceCollision : MonoBehaviour
{
    PlayerPhysic cachedPlayerPhysic = null;

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            cachedPlayerPhysic = collision.gameObject.GetComponent<PlayerPhysic>();
            cachedPlayerPhysic.Sliding();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            cachedPlayerPhysic.StopSliding();
        }
    }

    void OnDestroy()
    {
        if (cachedPlayerPhysic != null)
        {
            cachedPlayerPhysic.StopSliding();
            cachedPlayerPhysic = null;
        }
    }
}

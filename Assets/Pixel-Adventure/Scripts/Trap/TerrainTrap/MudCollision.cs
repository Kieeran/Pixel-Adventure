using UnityEngine;

public class MudCollision : MonoBehaviour
{
    PlayerPhysic cachedPlayerPhysic = null;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            cachedPlayerPhysic = collision.gameObject.GetComponent<PlayerPhysic>();

            ContactPoint2D contact = collision.GetContact(0);
            if (contact.normal.y < -0.7f)
            {
                cachedPlayerPhysic.ResistMovement(new Vector2(1, 0));
            }
            if (contact.normal.x != 0)
            {
                cachedPlayerPhysic.ResistMovement(new Vector2(0, 1));
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            cachedPlayerPhysic.ClearResistance();
            cachedPlayerPhysic = null;
        }
    }

    void OnDestroy()
    {
        if (cachedPlayerPhysic != null)
        {
            cachedPlayerPhysic.ClearResistance();
            cachedPlayerPhysic = null;
        }
    }
}

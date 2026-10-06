using Unity.VisualScripting;
using UnityEngine;

public class SandCollision : MonoBehaviour
{
    [SerializeField] float dragPercentage;
    PlayerPhysic cachedPlayerPhysic = null;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            cachedPlayerPhysic = collision.gameObject.GetComponent<PlayerPhysic>();
            cachedPlayerPhysic.ResistMovement(new Vector2(dragPercentage, 0));
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            cachedPlayerPhysic.ClearExternalForce();
            cachedPlayerPhysic = null;
        }
    }

    void OnDestroy()
    {
        if (cachedPlayerPhysic != null)
        {
            cachedPlayerPhysic.ClearExternalForce();
            cachedPlayerPhysic = null;
        }
    }
}

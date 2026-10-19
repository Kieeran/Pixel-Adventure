using Unity.VisualScripting;
using UnityEngine;

public class SandCollision : MonoBehaviour
{
    [SerializeField] float resistanceRatio;
    PlayerPhysic cachedPlayerPhysic = null;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            cachedPlayerPhysic = collision.gameObject.GetComponent<PlayerPhysic>();
            cachedPlayerPhysic.ResistMovement(new Vector2(resistanceRatio, 0));
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

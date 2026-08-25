using UnityEngine;

public class BoxCollision : MonoBehaviour
{
    [SerializeField] Box box;

    public Collider2D _collider;
    public float cachedColliderRadius;

    void OnValidate()
    {
        if (transform.TryGetComponent<Box>(out var box)) this.box = box;
        cachedColliderRadius = _collider.bounds.extents.magnitude;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            ContactPoint2D contact = collision.GetContact(0);
            if (contact.normal.y != 0)
            {
                box.IsCollided();

                if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
                {
                    if (contact.normal.y > 0) player.playerMovement.KnockBackByBox(false);
                    else player.playerMovement.KnockBackByBox(true);
                }
                else
                {
                    Debug.Log("Can get player ref when contact with box");
                }
            }
        }
    }
}
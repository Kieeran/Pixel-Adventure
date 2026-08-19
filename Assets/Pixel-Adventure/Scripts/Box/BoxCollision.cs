using UnityEngine;

public class BoxCollision : MonoBehaviour
{
    [SerializeField] Box box;
    [SerializeField] Collider2D _collider;

    void OnValidate()
    {
        if (TryGetComponent<Collider2D>(out var collider)) _collider = collider;
        if (transform.parent.TryGetComponent<Box>(out var box)) this.box = box;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            ContactPoint2D contact = collision.GetContact(0);
            if (contact.normal.y != 0)
            {
                box.IsCollided();
            }
        }
    }
}
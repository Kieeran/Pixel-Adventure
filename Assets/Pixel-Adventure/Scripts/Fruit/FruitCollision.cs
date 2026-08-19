using UnityEngine;

public class FruitCollision : MonoBehaviour
{
    [SerializeField] Fruit fruit;
    [SerializeField] Collider2D _collider;
    [SerializeField] Rigidbody2D rb;

    void OnValidate()
    {
        if (TryGetComponent<Collider2D>(out var collider)) _collider = collider;
        if (TryGetComponent<Rigidbody2D>(out var rigidbody)) rb = rigidbody;
        if (transform.parent.TryGetComponent<Fruit>(out var fruit)) this.fruit = fruit;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            fruit.IsCollected(true);
        }
    }
}

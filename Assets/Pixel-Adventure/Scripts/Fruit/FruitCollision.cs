using UnityEngine;

public class FruitCollision : MonoBehaviour
{
    [SerializeField] Fruit fruit;

    public Collider2D surfaceCollider;
    public float cachedColliderRadius;

    void OnValidate()
    {
        if (transform.TryGetComponent<Fruit>(out var fruit)) this.fruit = fruit;
        cachedColliderRadius = surfaceCollider.bounds.extents.magnitude;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            fruit.IsCollected(true);
        }
    }
}

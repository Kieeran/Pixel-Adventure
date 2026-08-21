using UnityEngine;

public class FruitCollision : MonoBehaviour
{
    [SerializeField] Fruit fruit;

    void OnValidate()
    {
        if (transform.TryGetComponent<Fruit>(out var fruit)) this.fruit = fruit;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            fruit.IsCollected(true);
        }
    }
}

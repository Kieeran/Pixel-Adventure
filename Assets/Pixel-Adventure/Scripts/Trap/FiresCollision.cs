using UnityEngine;

public class FiresCollision : MonoBehaviour
{
    public BoxCollider2D col;

    void OnValidate()
    {
        col = GetComponentInChildren<BoxCollider2D>();
    }
}

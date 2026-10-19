using System;
using UnityEngine;

public class SpikeHeadCollision : MonoBehaviour
{
    public event Action<Vector2> OnCharacterCollided;
    SpikeHead spikeHead;

    void Awake()
    {
        spikeHead = GetComponent<SpikeHead>();
        if (spikeHead == null)
        {
            Debug.LogError("SpikeHeadCollision requires a SpikeHead component on the same GameObject.", this);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleTerrainCollision(collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        HandleTerrainCollision(collision);
    }

    void HandleTerrainCollision(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Terrain") || spikeHead == null) return;

        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactPoint2D contact = collision.GetContact(i);
            if (spikeHead.IsBlockedByTerrain(contact.normal))
            {
                spikeHead.OnTerrainCollision(GetTerrainCollisionSide(contact.normal));
                return;
            }
        }
    }

    static SpikeHead.TerrainCollisionSide GetTerrainCollisionSide(Vector2 contactNormal)
    {
        if (Mathf.Abs(contactNormal.x) > Mathf.Abs(contactNormal.y))
        {
            return contactNormal.x > 0f
                ? SpikeHead.TerrainCollisionSide.Left
                : SpikeHead.TerrainCollisionSide.Right;
        }

        return contactNormal.y > 0f
            ? SpikeHead.TerrainCollisionSide.Bottom
            : SpikeHead.TerrainCollisionSide.Top;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            Vector2 hitPos = other.ClosestPoint(transform.parent.position);
            OnCharacterCollided?.Invoke(hitPos);
        }
    }
}

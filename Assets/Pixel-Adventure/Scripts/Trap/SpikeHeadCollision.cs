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
            if (spikeHead.IsBlockedByTerrain(collision.GetContact(i).normal))
            {
                spikeHead.OnTerrainCollision();
                return;
            }
        }
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

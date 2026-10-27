using System;
using System.Collections;
using UnityEngine;

public class Fruit : PlacedObject
{
    private static readonly int IsCollectedHash = Animator.StringToHash("IsCollected");
    [SerializeField] Animator animator;

    public Rigidbody2D rb;
    public FruitCollision fruitCollision;

    float originGravityScale;

    public virtual void IsCollected(bool b)
    {
        animator.SetBool(IsCollectedHash, b);
        StartCoroutine(HelperFunctions.WaitCurrentAnimationEnd(animator, () =>
        {
            PoolManager.Instance.Return(this);
        }));
    }

    void OnValidate()
    {
        animator = GetComponentInChildren<Animator>();
        if (TryGetComponent<Rigidbody2D>(out var rigidbody)) rb = rigidbody;
        if (TryGetComponent<FruitCollision>(out var collision)) fruitCollision = collision;
    }

    void Awake()
    {
        originGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
    }

    public void ApplyGravity()
    {
        rb.gravityScale = originGravityScale;
    }

    public override void OnSpawn()
    {
        rb.gravityScale = 0;
        fruitCollision.surfaceCollider.enabled = false;
    }

    public override void OnDespawn()
    {

    }
}
using System;
using System.Collections;
using UnityEngine;

public class Fruit : PlacedObject
{
    private static readonly int IsCollectedHash = Animator.StringToHash("IsCollected");
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;

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
    }

    public override void OnSpawn()
    {

    }

    public override void OnDespawn()
    {

    }
}
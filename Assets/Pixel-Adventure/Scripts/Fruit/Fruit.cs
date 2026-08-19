using System;
using System.Collections;
using UnityEngine;

public class Fruit : PlacedObject
{
    private static readonly int IsCollectedHash = Animator.StringToHash("IsCollected");
    [SerializeField] Animator animator;

    public virtual void IsCollected(bool b)
    {
        animator.SetBool(IsCollectedHash, b);
        StartCoroutine(HelpFunctions.WaitCurrentAnimationEnd(animator, () =>
        {
            PoolManager.Instance.Return(this);
        }));
    }

    void OnValidate()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public override void OnSpawn()
    {

    }

    public override void OnDespawn()
    {

    }
}
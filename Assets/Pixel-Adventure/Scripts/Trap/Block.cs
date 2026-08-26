using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : PlacedObject
{
    private static readonly int IsCollidedHash = Animator.StringToHash("IsCollided");
    [SerializeField] Animator animator;
    [SerializeField] BlockCollision blockCollision;
    [SerializeField] float knockCharacterUpForce;
    [SerializeField] float knockCharacterDownForce;

    void OnValidate()
    {
        animator = GetComponentInChildren<Animator>();
        if (TryGetComponent<BlockCollision>(out var collision)) blockCollision = collision;
    }

    void OnCharacterCollided(Vector2 direction)
    {
        PlayerController.Instance.playerMovement.ReboundVertically(
            direction,
            direction == Vector2.up ? knockCharacterUpForce : knockCharacterDownForce
        );

        animator.SetTrigger(IsCollidedHash);
        StartCoroutine(HelperFunctions.WaitCurrentAnimationEnd(animator, () =>
        {
            PoolManager.Instance.Return(this);
        }));
    }

    public override void OnSpawn()
    {
        blockCollision.OnCharacterCollided += OnCharacterCollided;
    }

    public override void OnDespawn()
    {
        blockCollision.OnCharacterCollided += OnCharacterCollided;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : PlacedObject
{
    private static readonly int IsCollidedHash = Animator.StringToHash("IsCollided");
    [SerializeField] Animator animator;
    [SerializeField] BlockCollision blockCollision;
    [SerializeField] BlockBreaks blockBreaks;
    [SerializeField] Transform skin;
    [SerializeField] Transform physic;
    [SerializeField] float knockCharacterUpForce;
    [SerializeField] float knockCharacterDownForce;

    void OnValidate()
    {
        animator = GetComponentInChildren<Animator>();
        blockBreaks = GetComponentInChildren<BlockBreaks>();
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
            OnBlockBreak();
            blockBreaks.Enable();
        }));
    }

    void OnBlockBreak()
    {
        skin.gameObject.SetActive(false);
        physic.gameObject.SetActive(false);
    }

    public override void OnSpawn()
    {
        skin.gameObject.SetActive(true);
        physic.gameObject.SetActive(true);
        blockBreaks.gameObject.SetActive(false);

        blockCollision.OnCharacterCollided += OnCharacterCollided;
        blockBreaks.OnAllBreaksDisappear += OnAllBreaksDisappear;
    }

    public override void OnDespawn()
    {
        blockCollision.OnCharacterCollided += OnCharacterCollided;
        blockBreaks.OnAllBreaksDisappear -= OnAllBreaksDisappear;
    }

    void OnAllBreaksDisappear()
    {
        PoolManager.Instance.Return(this);
    }
}
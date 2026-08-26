using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : PlacedObject
{
    private static readonly int IsCollidedHash = Animator.StringToHash("IsCollided");
    [SerializeField] Animator animator;
    [SerializeField] TrampolineCollision trampolineCollision;
    [SerializeField] float pushCharacterForce;

    void OnValidate()
    {
        animator = GetComponentInChildren<Animator>();
        trampolineCollision = GetComponent<TrampolineCollision>();
    }

    public override void OnSpawn()
    {
        trampolineCollision.OnCharacterCollided += OnCharacterCollided;
    }

    public override void OnDespawn()
    {
        trampolineCollision.OnCharacterCollided -= OnCharacterCollided;
    }

    void OnCharacterCollided()
    {
        animator.SetTrigger(IsCollidedHash);
        PlayerController.Instance.playerMovement.PushUpByTrampoline(pushCharacterForce);
    }
}
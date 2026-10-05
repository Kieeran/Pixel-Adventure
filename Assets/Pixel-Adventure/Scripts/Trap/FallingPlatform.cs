using System.Collections;
using UnityEngine;

public class FallingPlatform : PlacedObject
{
    private static readonly int IsOnHash = Animator.StringToHash("isOn");
    [SerializeField] FallingPlatformCollision fallingPlatformCollision;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem ps;
    [SerializeField] float fallDelay = 1f;
    [SerializeField] float gravityScale = 1f;
    [SerializeField] float returnToPoolDelay = 2f;

    Coroutine fallRoutine;
    bool isTriggered;
    bool isFalling;

    void OnValidate()
    {
        rb = GetComponent<Rigidbody2D>();
        ps = GetComponentInChildren<ParticleSystem>();
        animator = GetComponentInChildren<Animator>();
        fallingPlatformCollision = GetComponentInChildren<FallingPlatformCollision>();
    }

    public override void OnSpawn()
    {
        ResetPlatform();
        fallingPlatformCollision.OnCharacterCollided += OnCharacterCollided;
    }

    public override void OnDespawn()
    {
        fallingPlatformCollision.OnCharacterCollided -= OnCharacterCollided;
        if (fallRoutine != null)
        {
            StopCoroutine(fallRoutine);
            fallRoutine = null;
        }
    }

    void ResetPlatform()
    {
        rb.linearVelocity = Vector2.zero;
        animator.SetBool(IsOnHash, true);
        ToggleParticles(true);
        isTriggered = false;
        isFalling = false;
    }

    void FixedUpdate()
    {
        if (!isFalling) return;

        rb.linearVelocity += gravityScale * Time.fixedDeltaTime * Physics2D.gravity;
    }

    void OnCharacterCollided()
    {
        if (isTriggered) return;
        isTriggered = true;
        fallRoutine = StartCoroutine(FallAfterDelay());
    }

    IEnumerator FallAfterDelay()
    {
        yield return new WaitForSeconds(fallDelay);
        animator.SetBool(IsOnHash, false);
        ToggleParticles(false);
        rb.linearVelocity = Vector2.zero;
        isFalling = true;

        yield return new WaitForSeconds(returnToPoolDelay);
        ReturnToPool();
        fallRoutine = null;
    }

    void ReturnToPool()
    {
        isFalling = false;
        rb.linearVelocity = Vector2.zero;
        PoolManager.Instance.Return(this);
    }

    void ToggleParticles(bool b)
    {
        var emission = ps.emission;
        emission.enabled = b;
    }
}

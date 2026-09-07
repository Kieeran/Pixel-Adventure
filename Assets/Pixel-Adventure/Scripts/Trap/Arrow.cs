using UnityEngine;

public class Arrow : PlacedObject
{
    private static readonly int IsCollidedHash = Animator.StringToHash("IsCollided");
    [SerializeField] Animator animator;
    [SerializeField] ArrowCollision arrowCollision;
    [SerializeField] Transform physic;
    [SerializeField] float pushCharacterForce;

    void OnValidate()
    {
        animator = GetComponentInChildren<Animator>();
        arrowCollision = GetComponent<ArrowCollision>();
    }

    public override void OnSpawn()
    {
        physic.gameObject.SetActive(true);
        arrowCollision.OnCharacterCollided += OnCharacterCollided;
    }

    public override void OnDespawn()
    {
        arrowCollision.OnCharacterCollided -= OnCharacterCollided;
    }

    void OnCharacterCollided()
    {
        physic.gameObject.SetActive(false);
        animator.SetTrigger(IsCollidedHash);
        PlayerController.Instance.playerMovement.ReboundVertically(Vector2.up, pushCharacterForce);
        StartCoroutine(HelperFunctions.WaitCurrentAnimationEnd(animator, () =>
        {
            PoolManager.Instance.Return(this);
        }));
    }
}

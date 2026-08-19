using UnityEngine;

public class Box : PlacedObject
{
    private static readonly int IsCollidedHash = Animator.StringToHash("IsCollided");
    [SerializeField] Animator animator;
    [SerializeField] int Hp;

    int OriginHp => Hp;

    void OnValidate()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void IsCollided()
    {
        animator.SetTrigger(IsCollidedHash);
        StartCoroutine(HelpFunctions.WaitCurrentAnimationEnd(animator, () =>
        {
            Hp--;
            if (Hp <= 0)
            {
                PoolManager.Instance.Return(this);
            }
        }));
    }

    public override void OnSpawn()
    {
        Hp = OriginHp;
    }

    public override void OnDespawn()
    {

    }
}
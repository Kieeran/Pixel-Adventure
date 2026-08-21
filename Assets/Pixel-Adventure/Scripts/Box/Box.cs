using UnityEngine;

public class Box : PlacedObject
{
    private static readonly int IsCollidedHash = Animator.StringToHash("IsCollided");
    [SerializeField] Animator animator;
    [SerializeField] int Hp;
    [SerializeField] Breaks breaks;
    [SerializeField] Transform skin;
    [SerializeField] Transform physic;

    int originHp;

    void Awake()
    {
        originHp = Hp;
    }

    void OnValidate()
    {
        animator = GetComponentInChildren<Animator>();
        breaks = GetComponentInChildren<Breaks>();
    }

    public void IsCollided()
    {
        animator.SetTrigger(IsCollidedHash);
        StartCoroutine(HelpFunctions.WaitCurrentAnimationEnd(animator, () =>
        {
            Hp--;
            if (Hp < 0) return;
            if (Hp == 0)
            {
                OnBoxBreak();
                breaks.Enable();
            }
        }));
    }

    void OnBoxBreak()
    {
        skin.gameObject.SetActive(false);
        physic.gameObject.SetActive(false);
    }

    void OnAllBreaksDisappear()
    {
        PoolManager.Instance.Return(this);
    }

    public override void OnSpawn()
    {
        breaks.OnAllBreaksDisappear += OnAllBreaksDisappear;

        Hp = originHp;
        skin.gameObject.SetActive(true);
        physic.gameObject.SetActive(true);
        breaks.gameObject.SetActive(false);
    }

    public override void OnDespawn()
    {
        breaks.OnAllBreaksDisappear -= OnAllBreaksDisappear;
    }
}
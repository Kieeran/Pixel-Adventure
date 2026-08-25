using UnityEngine;

public class Box : PlacedObject
{
    private static readonly int IsCollidedHash = Animator.StringToHash("IsCollided");
    [SerializeField] Animator animator;
    [SerializeField] int Hp;
    [SerializeField] Breaks breaks;
    [SerializeField] Transform skin;
    [SerializeField] Transform physic;

    [HideInInspector] public BoxCollision boxCollision;
    public BoxRewardData boxRewardDataWhenBroken;
    public BoxRewardData boxRewardDataWhenHit;

    int originHp;

    void Awake()
    {
        originHp = Hp;
    }

    void OnValidate()
    {
        animator = GetComponentInChildren<Animator>();
        breaks = GetComponentInChildren<Breaks>();
        if (TryGetComponent<BoxCollision>(out var collision)) boxCollision = collision;
    }

    public void IsCollided()
    {
        animator.SetTrigger(IsCollidedHash);
        StartCoroutine(HelperFunctions.WaitCurrentAnimationEnd(animator, () =>
        {
            Hp--;
            if (Hp > 0)
            {
                EventChannel.Instance.OnBoxHit?.Invoke(this);
            }
            else if (Hp < 0)
            {
                return;
            }
            else
            {
                OnBoxBreak();
                breaks.Enable();
            }
        }));
    }

    void OnBoxBreak()
    {
        EventChannel.Instance.OnBoxBroken?.Invoke(this);

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
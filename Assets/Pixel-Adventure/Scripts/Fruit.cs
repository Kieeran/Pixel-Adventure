using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : PlacedObject
{
    private static readonly int IsCollectedHash = Animator.StringToHash("IsCollected");

    [SerializeField] Collider2D _collider;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;

    protected bool _isTrigger;
    protected int _gravityScale;

    public virtual void SetIsCollected(bool b)
    {
        animator.SetBool(IsCollectedHash, b);
        StartCoroutine(WaitAnimationEnd(() =>
        {
            PoolManager.Instance.Return(this);
        }));
    }

    public virtual void SetIsTrigger(bool b) { _collider.isTrigger = b; }
    public virtual bool GetIsTrigger() { return _isTrigger; }

    public virtual void SetGravityScale(float gs) { rb.gravityScale = gs; }
    public virtual float GetGravityScale() { return _gravityScale; }

    public void FirstBoost(float power, Rigidbody2D rb)
    {
        rb.linearVelocity = new Vector2(power, rb.linearVelocity.y + 10f);
    }

    protected virtual void Start()
    {
        // if (rb != null && rb.gravityScale > 0)
        // {
        //     FirstBoost(FruitManager.Instance.GetRandomPower() * 5, rb);
        // }
    }

    protected virtual void Update()
    {
        // if (_DoneCollecting)
        // {
        //     FruitManager.Instance.ReturnFruit(GetFruitID(), this);
        //     _DoneCollecting = false;
        // }
    }

    protected virtual void OnValidate()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        _collider = GetComponentInChildren<Collider2D>();
        animator = GetComponentInChildren<Animator>();
    }

    IEnumerator WaitAnimationEnd(Action onCompleted)
    {
        // Chờ 1 frame để Animator kịp chuyển state
        yield return null;

        // Lấy thông tin State hiện tại
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Đợi cho đến khi normalizedTime >= 1.0f (nghĩa là đã chạy xong 100% độ dài clip)
        while (stateInfo.normalizedTime < 1.0f)
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }

        // Tín hiệu hoàn tất!
        Debug.Log("Animation kết thúc từ Coroutine!");
        onCompleted?.Invoke();
    }

    public override void OnSpawn()
    {

    }

    public override void OnDespawn()
    {

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FruitID
{
    None, Apple, Bananas, Cherry, Kiwi, Melon, Orange, Pineapple, Strawberry
}

public class Fruit : MonoBehaviour
{
    [SerializeField] FruitID _fruitID;

    [SerializeField] Collider2D _collider;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb;

    protected bool IsCollected;
    protected bool _DoneCollecting;

    public FruitsData data;
    protected bool _isTrigger;
    protected int _gravityScale;

    public virtual bool GetIsCollected() { return IsCollected; }
    public virtual void SetIsCollected(bool b)
    {
        IsCollected = b;
    }

    public virtual void DoneCollecting()
    {
        _DoneCollecting = true;
    }

    public virtual FruitID GetFruitID() { return _fruitID; }

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
}
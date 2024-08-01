using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruits : MonoBehaviour
{
    private int fruitID;
    public Collider2D _collider;
    public Rigidbody2D rb;
    public Animator animator;

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

    public virtual int GetFruitID() { return fruitID; }
    public virtual void SetFruitID(int i) { fruitID = i; }

    public virtual void SetIsTrigger(bool b) { _collider.isTrigger = b; }
    public virtual bool GetIsTrigger() { return _isTrigger; }

    public virtual void SetGravityScale(float gs) { rb.gravityScale = gs; }
    public virtual float GetGravityScale() { return _gravityScale; }

    public void FirstBoost(float power, Rigidbody2D rb)
    {
        rb.velocity = new Vector2(power, rb.velocity.y + 10f);
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null && rb.gravityScale > 0)
        {
            FirstBoost(FruitManager.Instance.GetRandomPower() * 5, rb);
        }
    }

    protected virtual void Update()
    {
        if (_DoneCollecting)
        {
            FruitManager.Instance.ReturnFruit(GetFruitID(), this);
            _DoneCollecting = false;
        }
    }
}
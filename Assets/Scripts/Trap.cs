using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    protected int trapID;
    protected bool IsHit;
    public Animator animator;

    public TrapData data;
    public virtual int GetTrapID() { return trapID; }
    public virtual void SetTrapID(int i) { trapID = i; }

    public virtual void SetToggle(bool b)
    {
        Debug.Log("Do settoggle at trap");
    }

    public virtual void SetIsHit(bool b)
    {
        IsHit = b;
        animator.SetFloat("IsHit", 1);
    }

    public virtual void DoneHitting()
    {
        IsHit = false;
        animator.SetFloat("IsHit", 0);
    }

    protected virtual void Start()
    {

    }
}
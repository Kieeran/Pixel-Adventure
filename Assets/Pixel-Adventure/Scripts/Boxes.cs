using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxes : MonoBehaviour
{
    protected int boxID;
    protected bool IsHit;
    protected int boxHp;

    protected int fruitNum;

    public Animator animator;

    public virtual int GetBoxID() { return boxID; }
    public virtual void SetBoxID(int i) { boxID = i; }

    public virtual int GetBoxHP() { return boxHp; }
    public virtual void SetBoxHP(int i) { boxHp = i; }

    //public virtual bool GetIsHit() { return IsHit; }
    public virtual void SetIsHit(bool b)
    {
        IsHit = b;
        animator.SetFloat("IsHit", 1);
    }

    //public virtual void DropFruits(int number) { }

    public virtual void DoneHitting()
    {
        IsHit = false;
        animator.SetFloat("IsHit", -1);
        SetBoxHP(GetBoxHP() - 1);
        //DropFruits(1);
    }
}
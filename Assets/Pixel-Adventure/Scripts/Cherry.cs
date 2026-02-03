using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : Fruits
{
    public override void SetIsCollected(bool b)
    {
        base.SetIsCollected(b);
        animator.SetBool("IsCollected", b);
    }

    public override void DoneCollecting()
    {
        base.DoneCollecting();
        SetIsCollected(false);
    }

    protected override void Start()
    {
        base.Start();
        if (data != null)
        {
            SetFruitID(data.cherryID);
        }
        else
            Debug.Log("data null from cherry");
    }

    protected override void Update()
    {
        base.Update();
    }
}

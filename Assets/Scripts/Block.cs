using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Trap
{
    protected override void Start()
    {
        base.Start();
        if (data != null)
        {
            SetTrapID(data.blockID);
        }
        else
            Debug.Log("data null from block");
    }

    public override void DoneHitting()
    {
        base.DoneHitting();
        TrapsManager.Instance.ReturnTrap(this.GetTrapID(), this);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : Trap
{
    protected override void Start()
    {
        base.Start();
        if (data != null)
        {
            SetTrapID(data.trampolineID);
        }
        else
            Debug.Log("data null from trampoline");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Fan : Trap
{
    private float On_OffTime;
    public bool toggle;
    public float counter;
    public Vector2 forcePower;

    protected override void Start()
    {
        base.Start();
        if (data != null)
        {
            SetTrapID(data.fanID);
        }
        else
            Debug.Log("data null from fan");
        On_OffTime = 5f;
    }

    public Vector2 GetForcePower() { return forcePower; }

    public override void SetToggle(bool b)
    {
        base.SetToggle(b);
        if (b)
        {
            Debug.Log("Do settoggle at fan");
            forcePower = new Vector2(20, 0f);
        }
        else
        {
            Debug.Log("Not do settoggle at fan");
            forcePower = new Vector2(0, 20f);
        }
        toggle = b;
        animator.SetBool("toggle", toggle);
    }
    public bool GetToggle() { return toggle; }

    private void Update()
    {
        counter += Time.deltaTime;
        if (counter < On_OffTime) return;
        counter = 0f;
        toggle = !toggle;
        animator.SetBool("toggle", toggle);
    }
}
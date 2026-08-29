using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Fan : PlacedObject
{
    private static readonly int ToggleHash = Animator.StringToHash("Toggle");
    [SerializeField] Animator animator;

    void OnValidate()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public override void OnSpawn()
    {

    }
    public override void OnDespawn()
    {
        customData = null;
    }

    private float On_OffTime;
    public bool toggle;
    public float counter;
    public Vector2 forcePower;

    public Vector2 GetForcePower() { return forcePower; }

    // public override void SetToggle(bool b)
    // {
    //     base.SetToggle(b);
    //     if (b)
    //     {
    //         Debug.Log("Do settoggle at fan");
    //         forcePower = new Vector2(20, 0f);
    //     }
    //     else
    //     {
    //         Debug.Log("Not do settoggle at fan");
    //         forcePower = new Vector2(0, 20f);
    //     }
    //     toggle = b;
    //     animator.SetBool("toggle", toggle);
    // }
    public bool GetToggle() { return toggle; }

    public void Activate()
    {
        toggle = true;
        animator.SetBool(ToggleHash, toggle);
    }

    public void Deactivate()
    {
        toggle = false;
        animator.SetBool(ToggleHash, toggle);
    }
}
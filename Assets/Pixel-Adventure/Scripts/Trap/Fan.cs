using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Fan : PlacedObject
{
    private static readonly int ToggleHash = Animator.StringToHash("Toggle");
    [SerializeField] Animator animator;
    [SerializeField] Transform physic;
    [SerializeField] float pushVerticalPower;
    [SerializeField] float pushHorizontalPower;
    public Vector2 pushDirection = Vector2.zero;
    public bool isOn;

    void OnValidate()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public override void OnSpawn()
    {
        pushDirection = (customData as FanGroupData).pushDirection;
        physic.gameObject.SetActive(false);
    }

    public override void OnDespawn()
    {
        customData = null;
        isOn = false;
        pushDirection = Vector2.zero;
    }

    public void Activate()
    {
        isOn = true;
        animator.SetBool(ToggleHash, isOn);

        physic.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        isOn = false;
        animator.SetBool(ToggleHash, isOn);

        physic.gameObject.SetActive(false);
    }

    public float GetPushPower()
    {
        if (pushDirection == Vector2.up || pushDirection == Vector2.down)
        {
            return pushVerticalPower;
        }
        else if (pushDirection == Vector2.left || pushDirection == Vector2.right)
        {
            return pushHorizontalPower;
        }
        else return -1;
    }
}
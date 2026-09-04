using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Fan : PlacedObject
{
    private static readonly int ToggleHash = Animator.StringToHash("Toggle");
    [SerializeField] Animator animator;
    [SerializeField] Transform physic;
    [SerializeField] ParticleSystem ps;
    [SerializeField] float pushVerticalPower;
    [SerializeField] float pushHorizontalPower;
    public BoxCollider2D col;
    public Vector2 pushDirection = Vector2.zero;
    public bool isOn;

    void OnValidate()
    {
        animator = GetComponentInChildren<Animator>();
        col = GetComponentInChildren<BoxCollider2D>();
        ps = GetComponentInChildren<ParticleSystem>();
    }

    public override void OnSpawn()
    {
        FanGroupData data = customData as FanGroupData;
        pushDirection = data.pushDirection;
        SetColliderShape(data.colliderSize, data.colliderOffset);

        physic.gameObject.SetActive(false);
        ToggleFanParticles(false);
    }

    public override void OnDespawn()
    {
        customData = null;
        isOn = false;
        pushDirection = Vector2.zero;
        SetColliderShape(Vector2.one, Vector2.zero);
    }

    public void SetColliderShape(Vector2 size, Vector2 offset)
    {
        col.size = size;
        col.offset = offset;
    }

    public void Activate()
    {
        isOn = true;
        animator.SetBool(ToggleHash, isOn);
        physic.gameObject.SetActive(true);
        ToggleFanParticles(true);
    }

    public void Deactivate()
    {
        isOn = false;
        animator.SetBool(ToggleHash, isOn);
        physic.gameObject.SetActive(false);
        ToggleFanParticles(false);
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

    void ToggleFanParticles(bool b)
    {
        var emission = ps.emission;
        emission.enabled = b;
    }
}
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class PlayerInput : MonoBehaviour
{
    public InputAction moveAction;
    public InputAction jumpAction;

    public void Awake()
    {
        jumpAction.started +=
            ctx =>
        {
            Debug.Log("Jump!");
        };
    }

    public void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
    }

    public void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    public void Update()
    {
        moveAction.ReadValue<Vector2>();
    }
}
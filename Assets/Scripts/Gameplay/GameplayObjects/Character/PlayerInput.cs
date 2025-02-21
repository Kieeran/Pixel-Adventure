using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class PlayerInput : MonoBehaviour
{
    public InputAction moveAction;
    public InputAction jumpAction;

    public Vector2 move;
    public bool jump;

    public void Awake()
    {
        jumpAction.started +=
            ctx =>
        {
            jump = true;
            //Debug.Log("Start jump!");
        };

        jumpAction.canceled +=
            ctx =>
        {
            jump = false;
            //Debug.Log("Done jump!");
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
        move = moveAction.ReadValue<Vector2>();
    }
}
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class PlayerInput : MonoBehaviour
{
    public InputAction moveAction;

    public void OnEnable()
    {
        moveAction.Enable();
    }

    public void OnDisable()
    {
        moveAction.Disable();
    }

    public void Update()
    {
        var move = moveAction.ReadValue<Vector2>();

        Debug.Log(move);
    }
}
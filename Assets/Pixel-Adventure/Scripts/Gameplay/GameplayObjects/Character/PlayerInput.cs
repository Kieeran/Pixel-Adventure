using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public InputAction moveAction;
    public InputAction jumpAction;

    public Vector2 move = Vector2.zero;
    public bool isGrounded = false;
    public bool isJumpInAir = false;
    public bool isOnWall = false;
    public bool isContactLeftWall = false;

    public bool IsMovingHorizontal()
    {
        return PlayerController.Instance.playerMovement.playerRB.linearVelocityX != 0;
    }

    public void Awake()
    {
        jumpAction.started += ctx =>
        {
            PlayerController.Instance.OnJump?.Invoke();
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
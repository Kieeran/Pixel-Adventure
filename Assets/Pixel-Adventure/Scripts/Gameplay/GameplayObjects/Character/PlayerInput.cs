using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public InputAction moveAction;
    public InputAction jumpAction;

    public Vector2 move = Vector2.zero;
    public bool isGrounded = false;
    public bool isJumpInAir = false;

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

    // Chổ này về sau cần phải được xem xét lại
    // Vấn đề: bất đồng bộ giữa việc lấy input ở Update và dùng nó cho physic ở FixedUpdate
    public void FixedUpdate()
    {
        move = moveAction.ReadValue<Vector2>();
    }
}
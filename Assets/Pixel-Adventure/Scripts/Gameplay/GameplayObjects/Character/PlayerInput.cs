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
    public bool isExternallyPushed = false;

    public bool IsMovingHorizontal()
    {
        // Chỉ khi player điều khiển di chuyển thì mới tính là move
        // Còn không thì là bị tác động bởi yếu tố bên ngoài (external push)
        return PlayerController.Instance.playerInput.move.x != 0;
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
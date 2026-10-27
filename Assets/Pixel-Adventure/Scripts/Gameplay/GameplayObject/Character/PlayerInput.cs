using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 move = Vector2.zero;
    public bool isGrounded = false;
    public bool isJumpInAir = false;
    public bool isOnWall = false;
    public bool isContactLeftWall = false;
    public bool isExternallyForced = false;
    public bool isDead = false;
    public bool isResisted = false;
    public bool isSliding = false;

    public bool IsMovingHorizontal()
    {
        // Chỉ khi player điều khiển di chuyển thì mới tính là move
        // Còn không thì là bị tác động bởi yếu tố bên ngoài (external push)
        return PlayerController.Instance.playerInput.move.x != 0;
    }

    void Awake()
    {
        InputManager.Instance.OnJump += () =>
        {
            PlayerController.Instance.OnJump?.Invoke();
        };
    }

    void Update()
    {
        move = InputManager.Instance.GetMoveValue();
    }
}
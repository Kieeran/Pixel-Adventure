using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public InputAction moveAction;
    public InputAction jumpAction;
    public event Action OnJump;

    Vector2 move = Vector2.zero;

    public Vector2 GetMoveValue() { return move; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Init();
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

    void Init()
    {
        jumpAction.started += ctx =>
        {
            OnJump?.Invoke();
        };
    }

    void Update()
    {
        move = moveAction.ReadValue<Vector2>();
    }
}

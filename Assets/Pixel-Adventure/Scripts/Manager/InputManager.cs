using System;
using System.Collections;
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

    bool isInit = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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

    public IEnumerator Init()
    {
        if (PlatformUtil.Current == PlatformType.Mobile)
        {
            UIManager.Instance.mainCanvas.OnJumpButtonPressed += () =>
            {
                OnJump?.Invoke();
            };
        }
        else
        {
            jumpAction.started += ctx =>
            {
                OnJump?.Invoke();
            };
        }

        isInit = true;
        yield break;
    }

    void Update()
    {
        if (!isInit) return;

        if (PlatformUtil.Current == PlatformType.Mobile)
        {
            move = UIManager.Instance.mainCanvas.GetMoveButtonSignal();
        }
        else
        {
            move = moveAction.ReadValue<Vector2>();
        }
    }
}

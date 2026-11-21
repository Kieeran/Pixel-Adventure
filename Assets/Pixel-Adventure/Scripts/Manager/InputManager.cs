using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public InputAction resetLevelAction;
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

        resetLevelAction.Enable();
    }

    public void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();

        resetLevelAction.Disable();
    }

    public IEnumerator Init()
    {
        if (PlatformUtil.Current == PlatformType.Mobile)
        {
            UIManager.Instance.mainCanvas.gameplayPage.OnJumpButtonPressed += () =>
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

            resetLevelAction.started += ctx =>
            {
                EventChannel.Instance.OnChangeLevel?.Invoke();
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
            move = UIManager.Instance.mainCanvas.gameplayPage.GetMoveButtonSignal();
        }
        else
        {
            move = moveAction.ReadValue<Vector2>();
        }
    }
}

using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public PlayerInput playerInput;
    public PlayerMovement playerMovement;
    public PlayerCollision playerCollision;
    public PlayerAnimation playerAnimation;

    // FSM
    public StateMachine StateMachine { get; private set; }

    public IdleState IdleState { get; set; }
    public WalkState WalkState { get; set; }
    public InAirState InAirState { get; set; }
    public SlideOnWallState SlideOnWallState { get; set; }

    // Events
    public Action OnJump;
    public Action OnDoubleJump;

    public string CurrentState = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        InitFSM();
    }

    void OnValidate()
    {
        if (playerInput == null) playerInput = GetComponent<PlayerInput>();
        if (playerMovement == null) playerMovement = GetComponent<PlayerMovement>();
        if (playerCollision == null) playerCollision = GetComponent<PlayerCollision>();

        foreach (Transform child in transform)
        {
            if (playerAnimation == null && child.TryGetComponent<PlayerAnimation>(out var animation))
            {
                playerAnimation = animation;
            }
        }
    }

    void InitFSM()
    {
        StateMachine = new StateMachine();
        IdleState = new IdleState();
        WalkState = new WalkState();
        InAirState = new InAirState();
        SlideOnWallState = new SlideOnWallState();

        StateMachine.Initialize(InAirState);
    }

    void Update()
    {
        StateMachine.Update();
        CurrentState = StateMachine?.CurrentState?.Name;
    }

    void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }
}
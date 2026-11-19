using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInput playerInput;
    public PlayerPhysic playerPhysic;
    public PlayerCollision playerCollision;
    public PlayerAnimation playerAnimation;

    // FSM
    public StateMachine StateMachine { get; private set; }

    public IdleState IdleState { get; set; }
    public WalkState WalkState { get; set; }
    public InAirState InAirState { get; set; }
    public SlideOnWallState SlideOnWallState { get; set; }
    public DeadState DeadState { get; set; }

    // Events
    public Action OnJump;
    public Action OnDoubleJump;
    public Action OnDead;

    public string CurrentState = null;

    private void Awake()
    {
        playerInput = playerInput != null ? playerInput : GetComponent<PlayerInput>();
        playerPhysic = playerPhysic != null ? playerPhysic : GetComponent<PlayerPhysic>();
        playerCollision = playerCollision != null ? playerCollision : GetComponent<PlayerCollision>();
        playerAnimation = playerAnimation != null ? playerAnimation : GetComponent<PlayerAnimation>();

        playerInput.playerController = this;
        playerPhysic.playerController = this;
        playerCollision.playerController = this;
        playerAnimation.playerController = this;

        InitFSM();
    }

    void OnValidate()
    {
        if (playerInput == null) playerInput = GetComponent<PlayerInput>();
        if (playerPhysic == null) playerPhysic = GetComponent<PlayerPhysic>();
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
        StateMachine = new StateMachine(this);
        IdleState = new IdleState(StateMachine);
        WalkState = new WalkState(StateMachine);
        InAirState = new InAirState(StateMachine);
        SlideOnWallState = new SlideOnWallState(StateMachine);

        DeadState = new DeadState(StateMachine);
        StateMachine.AddAnyTransition(DeadState, () => playerInput.isDead);

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

    public void Reset()
    {
        StateMachine.ChangeState(InAirState);
        playerPhysic.Reset();
    }
}
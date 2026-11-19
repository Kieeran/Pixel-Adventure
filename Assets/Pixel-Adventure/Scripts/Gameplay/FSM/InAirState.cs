using UnityEngine;

public class InAirState : State
{
    public InAirState(StateMachine stateMachine) : base(stateMachine)
    {
        Name = StateName.InAir.ToString();
    }

    public override void HandleInput()
    {
        // InAir -> Idle
        if (StateMachine.PlayerController.playerInput.isGrounded == true && !StateMachine.PlayerController.playerInput.IsMovingHorizontal())
        {
            StateMachine.PlayerController.StateMachine.ChangeState(StateMachine.PlayerController.IdleState);
        }

        // InAir -> Walk
        if (StateMachine.PlayerController.playerInput.isGrounded == true && StateMachine.PlayerController.playerInput.IsMovingHorizontal())
        {
            StateMachine.PlayerController.StateMachine.ChangeState(StateMachine.PlayerController.WalkState);
        }

        // InAir -> SlideOnWall
        if (StateMachine.PlayerController.playerInput.isOnWall)
        {
            StateMachine.PlayerController.StateMachine.ChangeState(StateMachine.PlayerController.SlideOnWallState);
        }
    }

    public override void OnEnter()
    {
        StateMachine.PlayerController.OnJump += OnJumpInAir;
    }

    public override void OnExit()
    {
        StateMachine.PlayerController.OnJump -= OnJumpInAir;
    }

    void OnJumpInAir()
    {
        if (StateMachine.PlayerController.playerInput.isJumpInAir == false && StateMachine.PlayerController.playerInput.isGrounded == false)
        {
            if (StateMachine.PlayerController.playerInput.isExternallyForced) return;

            StateMachine.PlayerController.playerPhysic.JumpInAir();
            StateMachine.PlayerController.playerInput.isJumpInAir = true;
            StateMachine.PlayerController.OnDoubleJump?.Invoke();
        }
    }
}
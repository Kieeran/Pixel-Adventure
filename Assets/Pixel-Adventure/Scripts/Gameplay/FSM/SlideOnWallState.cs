using UnityEngine;

public class SlideOnWallState : State
{
    bool isBounceWall = false;
    public SlideOnWallState(StateMachine stateMachine) : base(stateMachine)
    {
        Name = StateName.SlideOnWall.ToString();
    }

    public override void HandleInput()
    {
        // SlideOnWall -> Idle
        if (StateMachine.PlayerController.playerInput.isGrounded && !StateMachine.PlayerController.playerInput.IsMovingHorizontal())
        {
            StateMachine.PlayerController.StateMachine.ChangeState(StateMachine.PlayerController.IdleState);
        }

        // SlideOnWall -> InAir
        if (!StateMachine.PlayerController.playerInput.isOnWall)
        {
            StateMachine.PlayerController.StateMachine.ChangeState(StateMachine.PlayerController.InAirState);
        }
    }

    public override void OnFixedUpdate()
    {
        if (StateMachine.PlayerController.playerInput.isExternallyForced)
        {
            StateMachine.PlayerController.playerInput.isOnWall = false;
            return;
        }

        if (isBounceWall)
        {
            if (!StateMachine.PlayerController.playerInput.IsMovingHorizontal())
            {
                StateMachine.PlayerController.playerInput.isOnWall = false;
            }
            return;
        }

        StateMachine.PlayerController.playerPhysic.SlideOnWall();
        StateMachine.PlayerController.playerPhysic.MoveHorizontal(StateMachine.PlayerController.playerInput.move.x);
    }

    public override void OnEnter()
    {
        isBounceWall = false;
        StateMachine.PlayerController.OnJump += OnJump;
    }

    public override void OnExit()
    {
        StateMachine.PlayerController.OnJump -= OnJump;
    }

    void OnJump()
    {
        if (isBounceWall) return;

        StateMachine.PlayerController.playerPhysic.JumpFromWall();
        isBounceWall = true;
    }
}
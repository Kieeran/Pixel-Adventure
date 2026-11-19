using UnityEngine;

public class WalkState : State
{
    public WalkState(StateMachine stateMachine) : base(stateMachine)
    {
        Name = StateName.Walk.ToString();
    }

    public override void HandleInput()
    {
        // Walk -> Idle
        if (!StateMachine.PlayerController.playerInput.IsMovingHorizontal())
        {
            StateMachine.PlayerController.StateMachine.ChangeState(StateMachine.PlayerController.IdleState);
        }

        // Walk -> InAir
        if (StateMachine.PlayerController.playerInput.isGrounded == false)
        {
            StateMachine.PlayerController.StateMachine.ChangeState(StateMachine.PlayerController.InAirState);
        }
    }

    public override void OnEnter()
    {
        StateMachine.PlayerController.OnJump += OnJump;
    }

    public override void OnExit()
    {
        StateMachine.PlayerController.OnJump -= OnJump;
    }

    void OnJump()
    {
        StateMachine.PlayerController.playerPhysic.Jump();
    }
}
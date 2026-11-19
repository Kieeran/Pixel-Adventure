using UnityEngine;

public class IdleState : State
{
    public IdleState(StateMachine stateMachine) : base(stateMachine)
    {
        Name = StateName.Idle.ToString();
    }

    public override void HandleInput()
    {
        // Idle -> Walk
        if (StateMachine.PlayerController.playerInput.IsMovingHorizontal())
        {
            StateMachine.PlayerController.StateMachine.ChangeState(StateMachine.PlayerController.WalkState);
        }

        // Idle -> InAir
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
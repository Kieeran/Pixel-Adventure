using UnityEngine;

public class IdleState : State
{
    public IdleState()
    {
        Name = StateName.Idle.ToString();
    }

    public override void HandleInput()
    {
        // Idle -> Walk
        if (PlayerController.Instance.playerInput.IsMovingHorizontal())
        {
            PlayerController.Instance.StateMachine.ChangeState(PlayerController.Instance.WalkState);
        }

        // Idle -> InAir
        if (PlayerController.Instance.playerInput.isGrounded == false)
        {
            PlayerController.Instance.StateMachine.ChangeState(PlayerController.Instance.InAirState);
        }
    }

    public override void OnEnter()
    {
        PlayerController.Instance.OnJump += OnJump;
    }

    public override void OnExit()
    {
        PlayerController.Instance.OnJump -= OnJump;
    }

    void OnJump()
    {
        PlayerController.Instance.playerMovement.Jump();
    }
}
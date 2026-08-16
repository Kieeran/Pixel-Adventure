using UnityEngine;

public class WalkState : State
{
    public WalkState()
    {
        Name = StateName.Walk.ToString();
    }

    public override void HandleInput()
    {
        // Walk -> Idle
        if (!PlayerController.Instance.playerInput.IsMovingHorizontal())
        {
            PlayerController.Instance.StateMachine.ChangeState(PlayerController.Instance.IdleState);
        }

        // Walk -> InAir
        if (PlayerController.Instance.playerInput.isGrounded == false)
        {
            PlayerController.Instance.StateMachine.ChangeState(PlayerController.Instance.InAirState);
        }
    }

    public override void OnFixedUpdate()
    {
        PlayerController.Instance.playerMovement.MoveHorizontal(PlayerController.Instance.playerInput.move.x);
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
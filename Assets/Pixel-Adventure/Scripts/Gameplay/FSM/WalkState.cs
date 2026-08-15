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
        if (!PlayerController.Instance.playerInput.IsMoving())
        {
            PlayerController.Instance.StateMachine.ChangeState(PlayerController.Instance.IdleState);
        }

        // Walk -> InAir
        if (PlayerController.Instance.playerInput.isGrounded == false)
        {
            PlayerController.Instance.StateMachine.ChangeState(PlayerController.Instance.InAirState);
        }
    }
}
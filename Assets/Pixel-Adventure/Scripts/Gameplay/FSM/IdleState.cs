using UnityEngine;

public class IdleState : State
{
    public IdleState()
    {
        Name = StateName.Idle.ToString();
    }

    public override void HandleInput()
    {
        // Idle <-> Walk
        if (PlayerController.Instance.playerInput.move != Vector2.zero)
        {
            PlayerController.Instance.StateMachine.ChangeState(PlayerController.Instance.WalkState);
        }

        // Idle <-> InAir
        if (PlayerController.Instance.playerInput.isGrounded == false)
        {
            PlayerController.Instance.StateMachine.ChangeState(PlayerController.Instance.InAirState);
        }
    }
}
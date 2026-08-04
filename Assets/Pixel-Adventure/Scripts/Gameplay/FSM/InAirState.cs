using UnityEngine;

public class InAirState : State
{
    public InAirState()
    {
        Name = StateName.InAir.ToString();
    }

    public override void HandleInput()
    {
        // InAir <-> Idle
        if (PlayerController.Instance.playerInput.isGrounded == true && PlayerController.Instance.playerInput.move == Vector2.zero)
        {
            PlayerController.Instance.StateMachine.ChangeState(PlayerController.Instance.IdleState);
        }

        // InAir <-> Walk
        if (PlayerController.Instance.playerInput.isGrounded == true && PlayerController.Instance.playerInput.move != Vector2.zero)
        {
            PlayerController.Instance.StateMachine.ChangeState(PlayerController.Instance.WalkState);
        }
    }
}
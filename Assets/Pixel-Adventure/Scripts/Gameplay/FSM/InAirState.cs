using UnityEngine;

public class InAirState : State
{
    public InAirState()
    {
        Name = StateName.InAir.ToString();
    }

    public override void HandleInput()
    {
        // InAir -> Idle
        if (PlayerController.Instance.playerInput.isGrounded == true && !PlayerController.Instance.playerInput.IsMoving())
        {
            PlayerController.Instance.StateMachine.ChangeState(PlayerController.Instance.IdleState);
        }

        // InAir -> Walk
        if (PlayerController.Instance.playerInput.isGrounded == true && PlayerController.Instance.playerInput.IsMoving())
        {
            PlayerController.Instance.StateMachine.ChangeState(PlayerController.Instance.WalkState);
        }

        // InAir -> SlideOnWall
        if (PlayerController.Instance.playerInput.isOnWall)
        {
            PlayerController.Instance.StateMachine.ChangeState(PlayerController.Instance.SlideOnWallState);
        }
    }
}
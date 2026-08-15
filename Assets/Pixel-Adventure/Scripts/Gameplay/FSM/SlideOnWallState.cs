using UnityEngine;

public class SlideOnWallState : State
{
    public SlideOnWallState()
    {
        Name = StateName.SlideOnWall.ToString();
    }

    public override void HandleInput()
    {
        // SlideOnWall -> Idle
        if (PlayerController.Instance.playerInput.isGrounded && !PlayerController.Instance.playerInput.IsMoving())
        {
            PlayerController.Instance.StateMachine.ChangeState(PlayerController.Instance.IdleState);
        }

        // SlideOnWall -> InAir
        if (!PlayerController.Instance.playerInput.isOnWall && PlayerController.Instance.playerInput.IsMoving())
        {
            PlayerController.Instance.StateMachine.ChangeState(PlayerController.Instance.InAirState);
        }
    }
}
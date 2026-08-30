using UnityEngine;

public class SlideOnWallState : State
{
    bool isBounceWall = false;
    public SlideOnWallState()
    {
        Name = StateName.SlideOnWall.ToString();
    }

    public override void HandleInput()
    {
        // SlideOnWall -> Idle
        if (PlayerController.Instance.playerInput.isGrounded && !PlayerController.Instance.playerInput.IsMovingHorizontal())
        {
            PlayerController.Instance.StateMachine.ChangeState(PlayerController.Instance.IdleState);
        }

        // SlideOnWall -> InAir
        if (!PlayerController.Instance.playerInput.isOnWall)
        {
            PlayerController.Instance.StateMachine.ChangeState(PlayerController.Instance.InAirState);
        }
    }

    public override void OnFixedUpdate()
    {
        if (PlayerController.Instance.playerInput.isPushedByFan)
        {
            PlayerController.Instance.playerInput.isOnWall = false;
            return;
        }

        if (isBounceWall)
        {
            if (!PlayerController.Instance.playerInput.IsMovingHorizontal())
            {
                PlayerController.Instance.playerInput.isOnWall = false;
            }
            return;
        }

        PlayerController.Instance.playerMovement.SlideOnWall();
        PlayerController.Instance.playerMovement.MoveHorizontal(PlayerController.Instance.playerInput.move.x);
    }

    public override void OnEnter()
    {
        isBounceWall = false;
        PlayerController.Instance.OnJump += OnJump;
    }

    public override void OnExit()
    {
        PlayerController.Instance.OnJump -= OnJump;
    }

    void OnJump()
    {
        if (isBounceWall) return;

        PlayerController.Instance.playerMovement.JumpFromWall();
        isBounceWall = true;
    }
}
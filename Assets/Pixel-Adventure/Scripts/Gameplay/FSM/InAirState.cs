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
        if (PlayerController.Instance.playerInput.isGrounded == true && !PlayerController.Instance.playerInput.IsMovingHorizontal())
        {
            PlayerController.Instance.StateMachine.ChangeState(PlayerController.Instance.IdleState);
        }

        // InAir -> Walk
        if (PlayerController.Instance.playerInput.isGrounded == true && PlayerController.Instance.playerInput.IsMovingHorizontal())
        {
            PlayerController.Instance.StateMachine.ChangeState(PlayerController.Instance.WalkState);
        }

        // InAir -> SlideOnWall
        if (PlayerController.Instance.playerInput.isOnWall)
        {
            PlayerController.Instance.StateMachine.ChangeState(PlayerController.Instance.SlideOnWallState);
        }
    }

    public override void OnEnter()
    {
        PlayerController.Instance.OnJump += OnJumpInAir;
    }

    public override void OnExit()
    {
        PlayerController.Instance.OnJump -= OnJumpInAir;
    }

    void OnJumpInAir()
    {
        if (PlayerController.Instance.playerInput.isJumpInAir == false && PlayerController.Instance.playerInput.isGrounded == false)
        {
            if (PlayerController.Instance.playerInput.isExternallyPushed) return;

            PlayerController.Instance.playerMovement.JumpInAir();
            PlayerController.Instance.playerInput.isJumpInAir = true;
            PlayerController.Instance.OnDoubleJump?.Invoke();
        }
    }
}
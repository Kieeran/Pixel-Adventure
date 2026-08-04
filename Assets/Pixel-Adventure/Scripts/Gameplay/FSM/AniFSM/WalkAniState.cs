using UnityEngine;

public class WalkAniState : AniState
{
    public WalkAniState(StateMachine stateMachine)
    {
        Name = AniStateName.AniWalk.ToString();
        Machine = stateMachine;
        IdStateHash = Animator.StringToHash("Walk");
    }

    public override void OnEnter()
    {
        PlayerController.Instance.playerAnimation.PlayAnimation(PlayerController.Instance.playerAnimation.WalkAniState.IdStateHash);
    }

    public override void HandleInput()
    {
        // Walk <-> Idle
        if (PlayerController.Instance.StateMachine.CurrentState.Name == StateName.Idle.ToString())
        {
            Machine.ChangeState(PlayerController.Instance.playerAnimation.IdleAniState);
        }

        // // Walk <-> InAir
        // if (PlayerController.Instance.playerInput.isGrounded == false)
        // {
        //     PlayerController.Instance.playerAnimation.AniStateMachine.ChangeState(PlayerController.Instance.playerAnimation.WalkAniState);
        // }
    }
}
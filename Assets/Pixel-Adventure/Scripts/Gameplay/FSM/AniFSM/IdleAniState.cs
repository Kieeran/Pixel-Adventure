using Unity.VisualScripting;
using UnityEngine;

public class IdleAniState : AniState
{
    public IdleAniState(StateMachine stateMachine)
    {
        Name = AniStateName.AniIdle.ToString();
        Machine = stateMachine;
        IdStateHash = Animator.StringToHash("Idle");
    }

    public override void OnEnter()
    {
        PlayerController.Instance.playerAnimation.PlayAnimation(PlayerController.Instance.playerAnimation.IdleAniState.IdStateHash);
    }

    public override void HandleInput()
    {
        // Idle <-> Walk
        if (PlayerController.Instance.StateMachine.CurrentState.Name == StateName.Walk.ToString())
        {
            Machine.ChangeState(PlayerController.Instance.playerAnimation.WalkAniState);
        }

        // // Idle <-> InAir
        // if (PlayerController.Instance.playerInput.isGrounded == false)
        // {
        //     PlayerController.Instance.playerAnimation.AniStateMachine.ChangeState(PlayerController.Instance.playerAnimation.IdleAniState);
        // }
    }
}
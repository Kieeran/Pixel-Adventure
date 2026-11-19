using UnityEngine;

public class DeadState : State
{
    public DeadState(StateMachine stateMachine) : base(stateMachine)
    {
        Name = StateName.Dead.ToString();
    }

    public override void OnFixedUpdate()
    {
        StateMachine.PlayerController.playerPhysic.LimitDeadSpin();
    }
}

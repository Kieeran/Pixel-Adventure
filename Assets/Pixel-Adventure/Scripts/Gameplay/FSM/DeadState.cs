using UnityEngine;

public class DeadState : State
{
    public DeadState()
    {
        Name = StateName.Dead.ToString();
    }

    public override void OnFixedUpdate()
    {
        PlayerController.Instance.playerPhysic.LimitDeadSpin();
    }
}

using UnityEngine;

public class GroundedState : HierarchicalState
{
    public GroundedState()
    {
        Name = "Grounded";

        var idleState = new IdleState(this);
        var walkState = new WalkState(this);

        RegisterSubState(idleState, isDefault: true);
        RegisterSubState(walkState);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}

public class IdleState : State
{
    GroundedState _parent;

    public IdleState(GroundedState parent)
    {
        Name = "Idle";
        _parent = parent;

        PlayerController.Instance.IdleState = this;
    }

    public override void HandleInput()
    {
        if (PlayerController.Instance.playerInput.move != Vector2.zero)
        {
            _parent.SwitchSubState(PlayerController.Instance.WalkState);
            PlayerController.Instance.playerAnimation.PlayAnimation(PlayerAnimation.RunStateHash);
        }
    }
}

public class WalkState : State
{
    GroundedState _parent;

    public WalkState(GroundedState parent)
    {
        Name = "Walk";
        _parent = parent;

        PlayerController.Instance.WalkState = this;
    }

    public override void HandleInput()
    {
        if (PlayerController.Instance.playerInput.move == Vector2.zero)
        {
            _parent.SwitchSubState(PlayerController.Instance.IdleState);
            PlayerController.Instance.playerAnimation.PlayAnimation(PlayerAnimation.IdleStateHash);
        }
    }
}
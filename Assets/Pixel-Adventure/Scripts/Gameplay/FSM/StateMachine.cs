public enum StateName
{
    Idle, Walk, InAir, SlideOnWall
}

public enum AniStateName
{
    AniIdle, AniWalk, AniJump, AniDouble_Jump, AniFall, AniSlideOnWall
}

public abstract class State
{
    public string Name { get; protected set; }

    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate()
    {
        PlayerController.Instance.playerMovement.MoveHorizontal(PlayerController.Instance.playerInput.move.x);
        PlayerController.Instance.playerMovement.HandleExternalPush(PlayerController.Instance.playerInput.move);
    }
    public virtual void OnExit() { }
    public virtual void HandleInput() { }
}

public class AniState : State
{
    public int IdStateHash;
    public StateMachine Machine;
}

public class StateMachine
{
    public State CurrentState { get; private set; }

    public void Initialize(State startingState)
    {
        CurrentState = startingState;
        CurrentState.OnEnter();
    }

    public void ChangeState(State newState)
    {
        CurrentState?.OnExit();
        CurrentState = newState;
        CurrentState.OnEnter();
    }

    public void Update()
    {
        CurrentState?.HandleInput();
        CurrentState?.OnUpdate();
    }

    public void FixedUpdate()
    {
        CurrentState?.OnFixedUpdate();
    }
}
using System;
using System.Collections.Generic;

public enum StateName
{
    Idle, Walk, InAir, SlideOnWall, Dead
}

public abstract class State
{
    public string Name { get; protected set; }

    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate()
    {
        PlayerController.Instance.playerPhysic.MoveHorizontal(PlayerController.Instance.playerInput.move.x);
        PlayerController.Instance.playerPhysic.HandleExternalPush(PlayerController.Instance.playerInput.move);
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
    private readonly List<(State to, Func<bool> condition)> anyTransitions = new();
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
        if (TryAnyTransition()) return;
        CurrentState?.HandleInput();
        CurrentState?.OnUpdate();
    }

    public void FixedUpdate()
    {
        CurrentState?.OnFixedUpdate();
    }

    public void AddAnyTransition(State to, Func<bool> condition)
    {
        anyTransitions.Add((to, condition));
    }

    private bool TryAnyTransition()
    {
        foreach (var (to, condition) in anyTransitions)
        {
            if (CurrentState != to && condition())
            {
                ChangeState(to);
                return true;
            }
        }
        return false;
    }
}
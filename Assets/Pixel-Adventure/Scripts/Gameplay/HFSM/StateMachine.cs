using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public string Name { get; protected set; }
    public HierarchicalState ParentState { get; set; } // Tham chiếu tới State cha (nếu có)

    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }
    public virtual void HandleInput() { }
}

public abstract class HierarchicalState : State
{
    protected State CurrentSubState;
    protected State DefaultSubState;
    protected List<State> SubStates = new List<State>();

    // Thêm Substate con và gán mối quan hệ Parent
    protected void RegisterSubState(State subState, bool isDefault = false)
    {
        SubStates.Add(subState);
        subState.ParentState = this;

        if (isDefault)
        {
            DefaultSubState = subState;
        }
    }

    // Chuyển đổi giữa các Substate CON trong phạm vi của Superstate này
    public void SwitchSubState(State newSubState)
    {
        CurrentSubState?.OnExit();
        CurrentSubState = newSubState;
        CurrentSubState.OnEnter();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        // Khi Superstate Enter, mặc định activate Substate mặc định của nó
        if (CurrentSubState == null && DefaultSubState != null)
        {
            CurrentSubState = DefaultSubState;
        }
        CurrentSubState?.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        // Chạy logic của Superstate trước, sau đó delegate xuống Substate
        CurrentSubState?.OnUpdate();
    }

    public override void OnExit()
    {
        // Exit Substate trước, rồi mới Exit Superstate
        CurrentSubState?.OnExit();
        CurrentSubState = null;
        base.OnExit();
    }

    public override void HandleInput()
    {
        base.HandleInput();
        CurrentSubState?.HandleInput();
    }
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
}
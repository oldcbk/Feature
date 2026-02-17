using System;
using UnityEngine;

[Serializable]
public class StateMachine
{
    public string state;

    BasePlayerState m_currentState;
    public BasePlayerState CurrentState
    {
        get { return m_currentState; }
        set
        {
            m_currentState = value; state = value.GetType().Name;
        }
    }

    public void Init(BasePlayerState state)
    {
        CurrentState = state;
    }

    // 应该是仅由BasePlayerState类对象调用
    public void ChangeState(BasePlayerState state)
    {
        Debug.Log($"ChangeState from {m_currentState.GetType().Name} to {state.GetType().Name}");
        m_currentState.OnExit();
        CurrentState = state;
        m_currentState.OnEnter();
    }

    public void OnUpdate()
    {
        m_currentState.OnUpdate();
    }
}

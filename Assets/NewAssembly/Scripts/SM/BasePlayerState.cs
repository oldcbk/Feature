using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BasePlayerState
{
    readonly protected StateMachine m_stateMachine;
    readonly protected Player m_player;

    public BasePlayerState(StateMachine stateMachine, Player player)
    {
        m_stateMachine = stateMachine;
        m_player = player;
    }

    public virtual void OnEnter() { Assert.IsTrue(false); }
    public virtual void OnUpdate() { Assert.IsTrue(false); }
    public virtual void OnExit() { Assert.IsTrue(false); }
}

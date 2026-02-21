using UnityEngine;

public class TestKatanaHero : MonoBehaviour
{
    // InputActions & Impl
    public NewActions m_IAs;
    public 纳刀SActionsImpl m_纳刀SA;
    public 纳刀MActionsImpl m_纳刀MA;
    public 拔刀SActionsImpl m_拔刀A;
    public 攻击SActionsImpl m_攻击A;
    // state
    public StateMachine m_SM;
    public 纳刀S m_纳刀S;
    public 翻滚M m_翻滚M;
    public 飞扑M m_飞扑M;
    public 拔刀M m_拔刀M;
    public 拔刀斩S m_拔刀斩S;
    public 拔刀S m_拔刀S;
    public 纳刀M m_纳刀M;

    public StateMachine.IState m_戳S;
    public StateMachine.IState m_气刃斩S;
    public StateMachine.IState m_看破斩S;
    public StateMachine.IState m_特殊纳刀S;
    public StateMachine.IState m_居合拔刀斩S;
    public StateMachine.IState m_居合拔刀气刃斩S;
    public StateMachine.IState m_放松M;


    public PN1 m_朝向;
    public bool m_纳刀中;
    public PN1 m_方向Cache;

    public void HandleMove(float moveSpeed) { }
}


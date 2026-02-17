using UnityEngine;

public class GroundState : BasePlayerState
{
    int MaxJumpCnt => m_player.m_maxJumpCnt;
    BasePlayerState AirState => m_player.m_airState;

    public GroundState(StateMachine stateMachine, Player player) : base(stateMachine, player)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("Enter  GroundState");
        OnUpdate();
    }

    public override void OnUpdate()
    {
        Debug.Log("Update GroundState");
        m_player.m_remainJumpCnt = MaxJumpCnt;
        if (!m_player.m_groundProbe.m_hit)
        {
            m_stateMachine.ChangeState(AirState);
        }
    }

    public override void OnExit()
    {
        Debug.Log("Exit   GroundState");
        m_player.m_remainJumpCnt = m_player.m_maxJumpCnt - 1;
    }
}

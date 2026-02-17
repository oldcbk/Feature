using UnityEngine;

public class AirState : BasePlayerState
{
    BasePlayerState GroundState => m_player.m_groundState;

    public AirState(StateMachine stateMachine, Player player) : base(stateMachine, player)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("Enter  AirState");
        OnUpdate();
    }

    public override void OnUpdate()
    {
        Debug.Log("Update AirState");
        if (m_player.m_groundProbe.m_hit)
        {
            m_stateMachine.ChangeState(GroundState);
        }
    }

    public override void OnExit()
    {
        Debug.Log("Exit   AirState");
    }
}

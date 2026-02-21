public class StateMachine
{
    public IState CurrState;
    public void ToState(IState state) { }
    public interface IState
    {
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
}
public class State : StateMachine.IState
{
    public readonly StateMachine m_SM;
    public State(StateMachine stateMachine)
    {
        m_SM = stateMachine;
    }
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
    public virtual void OnUpdate() { }
}
public class KatanaHeroState : State
{
    public readonly TestKatanaHero m_KH;
    public KatanaHeroState(StateMachine stateMachine, TestKatanaHero katanaHero) : base(stateMachine)
    {
        m_KH = katanaHero;
    }
}

public class 纳刀S : KatanaHeroState
{
    public 纳刀S(StateMachine stateMachine, TestKatanaHero katanaHero) : base(stateMachine, katanaHero)
    {
    }
    public override void OnEnter()
    {
        纳刀A.Enable();
        m_KH.m_纳刀中 = true;
    }
    public override void OnUpdate()
    {
        m_KH.m_方向Cache = 纳刀AImpl.m_方向;
        switch (纳刀AImpl.TryDo.Pop())
        {
        case 纳刀SActionsImpl.CanDo.翻滚: { m_SM.ToState(翻滚); break; }
        case 纳刀SActionsImpl.CanDo.飞扑: { m_SM.ToState(飞扑); break; }
        case 纳刀SActionsImpl.CanDo.拔刀: { m_SM.ToState(拔刀); break; }
        case 纳刀SActionsImpl.CanDo.拔刀斩: { m_SM.ToState(拔刀斩); break; }
        case 纳刀SActionsImpl.CanDo.气刃斩: { m_SM.ToState(气刃斩); break; }
        default: m_KH.HandleMove(纳刀AImpl.m_移动速度); break;
        }
    }
    public override void OnExit()
    {
        纳刀A.Disable();
        纳刀AImpl.TryDo.Pop();
    }

    NewActions IAs => m_KH.m_IAs;
    NewActions.纳刀SActions 纳刀A => IAs.纳刀S;
    纳刀SActionsImpl 纳刀AImpl => m_KH.m_纳刀SA;

    StateMachine.IState 翻滚 => m_KH.m_翻滚M;
    StateMachine.IState 飞扑 => m_KH.m_飞扑M;
    StateMachine.IState 拔刀 => m_KH.m_拔刀M;
    StateMachine.IState 拔刀斩 => m_KH.m_拔刀斩S;
    StateMachine.IState 气刃斩 => m_KH.m_气刃斩S;
}
public class 翻滚M : KatanaHeroState
{
    public float AnimDuration { set { m_animTimer.Set(value); } }
    public 翻滚M(StateMachine stateMachine, TestKatanaHero katanaHero) : base(stateMachine, katanaHero)
    {
        m_animTimer = new(m_KH, 1);
    }
    public override void OnEnter()
    {
        m_animTimer.Start();
    }
    public override void OnUpdate()
    {
        if (!m_animTimer.IsRunning) m_SM.ToState(m_KH.m_纳刀中 ? 纳刀 : 拔刀);
    }
    public override void OnExit()
    {
        m_animTimer.Stop();
    }

    Timer m_animTimer;
    StateMachine.IState 纳刀 => m_KH.m_纳刀S;
    StateMachine.IState 拔刀 => m_KH.m_拔刀S;
}
public class 飞扑M : KatanaHeroState
{
    public 飞扑M(StateMachine stateMachine, TestKatanaHero katanaHero) : base(stateMachine, katanaHero)
    {
        m_animTimer = new(m_KH, 2);
    }
    public override void OnEnter()
    {
        m_animTimer.Start();
    }
    public override void OnUpdate()
    {
        if (!m_animTimer.IsRunning) m_SM.ToState(纳刀);
    }
    public override void OnExit()
    {
        m_animTimer.Stop();
    }

    Timer m_animTimer;
    StateMachine.IState 纳刀 => m_KH.m_纳刀S;
}
public class 拔刀M : KatanaHeroState
{
    public 拔刀M(StateMachine stateMachine, TestKatanaHero katanaHero) : base(stateMachine, katanaHero)
    {
        m_animTimer = new(m_KH, 1);
    }
    public override void OnEnter()
    {
        m_animTimer.Start();
    }
    public override void OnUpdate()
    {
        if (!m_animTimer.IsRunning) m_SM.ToState(拔刀);
    }
    public override void OnExit()
    {
        m_animTimer.Stop();
    }

    Timer m_animTimer;
    StateMachine.IState 拔刀 => m_KH.m_拔刀S;
}
public class 拔刀斩S : KatanaHeroState
{
    public 拔刀斩S(StateMachine stateMachine, TestKatanaHero katanaHero) : base(stateMachine, katanaHero)
    {
        m_animTimer = new(m_KH, 1);
        m_animTimer.OnTimeUp += OnAnimTimeUp;
        m_waitTimer = new(m_KH, 1);
    }
    public override void OnEnter()
    {
        m_animTimer.Start();
        攻击A.Enable();
    }
    public override void OnUpdate()
    {
        if (m_canReadInputs)
        {
            m_KH.m_方向Cache = 攻击AImpl.方向;
            switch (攻击AImpl.TryDo.Pop())
            {
            case 攻击SActionsImpl.CanDo.翻滚: m_SM.ToState(翻滚); break;
            case 攻击SActionsImpl.CanDo.纳刀: m_SM.ToState(纳刀); break;
            case 攻击SActionsImpl.CanDo.戳: m_SM.ToState(戳); break;
            case 攻击SActionsImpl.CanDo.气刃斩: m_SM.ToState(气刃斩); break;
            case 攻击SActionsImpl.CanDo.看破斩: m_SM.ToState(看破斩); break;
            case 攻击SActionsImpl.CanDo.特殊纳刀: m_SM.ToState(特殊纳刀); break;
            default:
                {
                    if (!m_waitTimer.IsRunning) m_SM.ToState(放松);
                    break;
                }
            }
        }
    }
    public override void OnExit()
    {
        m_animTimer.Stop();
        m_waitTimer.Stop();
        攻击A.Disable();
    }

    NewActions IAs => m_KH.m_IAs;
    NewActions.攻击SActions 攻击A => IAs.攻击S;
    攻击SActionsImpl 攻击AImpl => m_KH.m_攻击A;
    StateMachine.IState 翻滚 => m_KH.m_翻滚M;
    StateMachine.IState 纳刀 => m_KH.m_纳刀M;
    StateMachine.IState 戳 => m_KH.m_戳S;
    StateMachine.IState 气刃斩 => m_KH.m_气刃斩S;
    StateMachine.IState 看破斩 => m_KH.m_看破斩S;
    StateMachine.IState 特殊纳刀 => m_KH.m_特殊纳刀S;
    StateMachine.IState 放松 => m_KH.m_放松M;

    readonly Timer m_animTimer;
    readonly Timer m_waitTimer;
    bool m_canReadInputs;

    void OnAnimTimeUp()
    {
        m_canReadInputs = true;
        m_waitTimer.Start();
    }
}
public class 拔刀S : KatanaHeroState
{
    public 拔刀S(StateMachine stateMachine, TestKatanaHero katanaHero) : base(stateMachine, katanaHero)
    {
    }
    public override void OnEnter()
    {
        拔刀A.Enable();
    }
    public override void OnUpdate()
    {
        m_KH.m_方向Cache = 拔刀AImpl.m_方向;
        switch (拔刀AImpl.TryDo.Pop())
        {
        case 拔刀SActionsImpl.CanDo.翻滚: m_SM.ToState(翻滚); break;
        case 拔刀SActionsImpl.CanDo.纳刀: m_SM.ToState(纳刀); break;
        case 拔刀SActionsImpl.CanDo.戳: m_SM.ToState(戳); break;
        case 拔刀SActionsImpl.CanDo.气刃斩: m_SM.ToState(气刃斩); break;
        default: m_KH.HandleMove(拔刀AImpl.m_移动速度); break;
        }
    }
    public override void OnExit()
    {
        拔刀A.Disable();
    }

    NewActions IAs => m_KH.m_IAs;
    NewActions.拔刀SActions 拔刀A => IAs.拔刀S;
    拔刀SActionsImpl 拔刀AImpl => m_KH.m_拔刀A;

    StateMachine.IState 翻滚 => m_KH.m_翻滚M;
    StateMachine.IState 纳刀 => m_KH.m_纳刀S;
    StateMachine.IState 戳 => m_KH.m_戳S;
    StateMachine.IState 气刃斩 => m_KH.m_气刃斩S;
}
public class 纳刀M : KatanaHeroState
{
    NewActions.纳刀MActions 纳刀A => m_newActions.纳刀M;

    readonly NewActions m_newActions;

    public 纳刀M(StateMachine stateMachine, TestKatanaHero katanaHero) : base(stateMachine, katanaHero)
    {
    }

    public void OnEnter()
    {
        纳刀A.Enable();
    }
    public void OnUpdate()
    {
    }
    public void OnExit()
    {
        纳刀A.Disable();
    }
}


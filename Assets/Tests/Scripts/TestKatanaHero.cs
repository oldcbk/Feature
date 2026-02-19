using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class TestKatanaHero : MonoBehaviour
{
    NewActions m_newActions;
    纳刀SActions m_纳刀SA;
    纳刀MActions m_纳刀MA;
    拔刀SActions m_拔刀A;
    攻击SActions m_攻击A;
}
public class StateMachine
{
    public void ToState(State state) { }
}
public class State
{
    public NewActions m_newActions;

    public State(NewActions newActions)
    {
        m_newActions = newActions;
    }
    public virtual void OnEnter() { }
    public virtual void OnUpdate() { }
    public virtual void OnExit() { }
}

public class 纳刀S : State
{
    NewActions.纳刀SActions 纳刀A => m_newActions.纳刀S;

    public 纳刀S(NewActions newActions) : base(newActions)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        纳刀A.Enable();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
    }
    public override void OnExit()
    {
        base.OnExit();
        纳刀A.Disable();
    }
}
public class 纳刀M : State
{
    NewActions.纳刀MActions 纳刀A => m_newActions.纳刀M;

    public 纳刀M(NewActions newActions) : base(newActions)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        纳刀A.Enable();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
    }
    public override void OnExit()
    {
        base.OnExit();
        纳刀A.Disable();
    }
}


/// <summary>+1/-1</summary>
public struct PN1
{
    public int value;
    public PN1(int v)
    {
        Assert.IsTrue(v != 0);
        value = Math.Sign(v);
    }
    public PN1(float v)
    {
        Assert.IsTrue(v != 0);
        value = Math.Sign(v);
    }
    public static implicit operator PN1(int v)
    {
        return new(v);
    }
    public static implicit operator PN1(float v)
    {
        return new(v);
    }
}

public class 纳刀SActions : NewActions.I纳刀SActions
{
    // public getter
    public ICanDoBuffer TryDo => m_tryDo;
    public float m_移动速度;
    public PN1 m_翻滚方向;

    public enum CanDo
    {
        空, 翻滚, 飞扑, 拔刀, 拔刀斩, 气刃斩
    }
    public interface ICanDoBuffer
    {
        public CanDo Peak();
        public CanDo Pop();
    }

    // private getter
    NewActions m_newActions;
    NewActions.纳刀SActions 纳刀A => m_newActions.纳刀S;
    float 奔跑速度;
    PN1 朝向;
    InputAction 飞扑IA => 纳刀A.飞扑;
    class CanDoBuffer : ICanDoBuffer
    {
        CanDo m_canDo;
        public void Push(CanDo canDo)
        {
            m_canDo = canDo;
        }
        public CanDo Peak()
        {
            return m_canDo;
        }
        public CanDo Pop()
        {
            var temp = m_canDo;
            m_canDo = CanDo.空;
            return temp;
        }
    }

    // private
    float m_移动;
    bool m_奔跑;
    readonly CanDoBuffer m_tryDo;

    public 纳刀SActions(NewActions newActions)
    {
        m_newActions = newActions;
        m_tryDo = new();
        纳刀A.SetCallbacks(this);
    }
    public void On移动(InputAction.CallbackContext context)
    {
        m_移动 = context.ReadValue<float>();
        if (m_移动 == 0)
        {
            飞扑IA.Disable();
        }
        else
        {
            飞扑IA.Enable();
        }
        if (m_奔跑)
        {
            m_移动速度 = Math.Sign(m_移动) * 奔跑速度;
        }
        else
        {
            m_移动速度 = m_移动;
        }
    }
    public void On奔跑(InputAction.CallbackContext context)
    {
        m_奔跑 = context.ReadValue<bool>();
    }
    public void On翻滚(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        m_翻滚方向 = m_移动 == 0 ? 朝向 : m_移动;
        m_tryDo.Push(CanDo.翻滚);
    }
    public void On飞扑(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        m_tryDo.Push(CanDo.翻滚);
    }
    public void On拔刀(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        if (m_移动 == 0)
        {
            m_tryDo.Push(CanDo.拔刀);
        }
        else
        {
            m_tryDo.Push(CanDo.拔刀斩);
        }
    }
    public void On气刃斩(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        m_tryDo.Push(CanDo.气刃斩);
    }
}
public class 纳刀MActions : NewActions.I纳刀MActions
{
    public float m_移动速度;

    NewActions NewActions;
    NewActions.纳刀MActions 纳刀A => NewActions.纳刀M;

    public 纳刀MActions()
    {
        纳刀A.SetCallbacks(this);
    }
    public void On移动(InputAction.CallbackContext context)
    {
        m_移动速度 = context.ReadValue<float>();
    }
}
public class 拔刀SActions : NewActions.I拔刀SActions
{
    // public getter
    public ICanDoBuffer TryDo => m_tryDo;
    public float m_移动速度;
    public PN1 m_翻滚方向;

    public enum CanDo
    {
        空, 翻滚, 纳刀, 戳, 气刃斩
    }
    public interface ICanDoBuffer
    {
        public CanDo Peak();
        public CanDo Pop();
    }

    // private getter
    NewActions m_newActions;
    NewActions.拔刀SActions 拔刀A => m_newActions.拔刀S;
    PN1 朝向;

    class CanDoBuffer : ICanDoBuffer
    {
        CanDo m_canDo;
        public void Push(CanDo canDo)
        {
            m_canDo = canDo;
        }
        public CanDo Peak()
        {
            return m_canDo;
        }
        public CanDo Pop()
        {
            var temp = m_canDo;
            m_canDo = CanDo.空;
            return temp;
        }
    }

    // private
    readonly CanDoBuffer m_tryDo;

    public 拔刀SActions(NewActions newActions)
    {
        m_newActions = newActions;
        m_tryDo = new();
        拔刀A.SetCallbacks(this);
    }
    public void On移动(InputAction.CallbackContext context)
    {
        m_移动速度 = context.ReadValue<float>();
    }
    public void On翻滚(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        m_翻滚方向 = m_移动速度 == 0 ? 朝向 : m_移动速度;
        m_tryDo.Push(CanDo.翻滚);
    }
    public void On纳刀(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        m_tryDo.Push(CanDo.纳刀);
    }
    public void On戳(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        m_tryDo.Push(CanDo.戳);
    }
    public void On气刃斩(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        m_tryDo.Push(CanDo.气刃斩);
    }
}
public class 攻击SActions : NewActions.I攻击SActions
{
    // public getter
    public ICanDoBuffer TryDo => m_tryDo;
    public PN1 m_翻滚方向;

    public enum CanDo
    {
        空, 翻滚, 纳刀, 戳, 气刃斩, 看破斩, 特殊纳刀, 居合拔刀斩, 居合拔刀气刃斩
    }
    public interface ICanDoBuffer
    {
        public CanDo Peak();
        public CanDo Pop();
    }

    // private getter
    NewActions m_newActions;
    NewActions.攻击SActions 攻击A => m_newActions.攻击S;
    PN1 朝向;

    class CanDoBuffer : ICanDoBuffer
    {
        CanDo m_canDo;
        public void Push(CanDo canDo)
        {
            m_canDo = canDo;
        }
        public CanDo Peak()
        {
            return m_canDo;
        }
        public CanDo Pop()
        {
            var temp = m_canDo;
            m_canDo = CanDo.空;
            return temp;
        }
    }

    // private
    readonly CanDoBuffer m_tryDo;
    float m_方向;

    public 攻击SActions(NewActions newActions)
    {
        m_newActions = newActions;
        m_tryDo = new();
        攻击A.SetCallbacks(this);
    }
    public void On方向(InputAction.CallbackContext context)
    {
        m_方向 = context.ReadValue<float>();
    }
    public void On翻滚(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        m_翻滚方向 = m_方向 == 0 ? 朝向 : m_方向;
        m_tryDo.Push(CanDo.翻滚);
    }
    public void On纳刀(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        m_tryDo.Push(CanDo.纳刀);
    }
    public void On戳(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        m_tryDo.Push(CanDo.戳);
    }
    public void On气刃斩(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        m_tryDo.Push(CanDo.气刃斩);
    }
    public void On看破斩(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        m_tryDo.Push(CanDo.看破斩);
    }
    public void On特殊纳刀(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        m_tryDo.Push(CanDo.特殊纳刀);
    }
    public void On居合拔刀斩(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        m_tryDo.Push(CanDo.居合拔刀斩);
    }
    public void On居合拔刀气刃斩(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        m_tryDo.Push(CanDo.居合拔刀气刃斩);
    }
}

public class Timer
{
    public event Action OnTimeUp;

    MonoBehaviour m_MB;
    float m_seconds;

    float m_countDown;
    bool m_isRunning;
    Coroutine m_coroutine;

    public Timer(MonoBehaviour monoBehaviour = null, float seconds = 0)
    {
        m_MB = monoBehaviour;
        m_seconds = seconds;
    }

    public void Init(MonoBehaviour monoBehaviour, float seconds = 0)
    {
        Assert.IsFalse(m_isRunning);
        m_MB = monoBehaviour;
        m_seconds = seconds;
    }

    public void Start()
    {
        m_isRunning = true;
        m_countDown = m_seconds;
        m_coroutine = m_MB.StartCoroutine(foo());
    }

    public void Stop()
    {
        m_MB.StopCoroutine(m_coroutine);
        m_isRunning = false;
    }

    IEnumerator foo()
    {
        yield return new WaitForSeconds(m_countDown);
        m_isRunning = false;
        OnTimeUp();
    }
}

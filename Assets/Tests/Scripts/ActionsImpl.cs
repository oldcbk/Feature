using System;
using UnityEngine.InputSystem;

public interface IActionsImpl
{
    NewActions GetNewActions();
    float Get奔跑速度();
    PN1 Get朝向();
}
public class 纳刀SActionsImpl : NewActions.I纳刀SActions
{
    // public getter
    public ICanDoBuffer TryDo => m_tryDo;
    public float m_移动速度;
    public PN1 m_方向;

    public enum CanDo
    {
        空, 翻滚, 飞扑, 拔刀, 拔刀斩, 气刃斩
    }
    public interface ICanDoBuffer
    {
        public CanDo Peak();
        public CanDo Pop();
    }

    public void Register(IActionsImpl impl)
    {
        GetNewActions = impl.GetNewActions;
        Get奔跑速度 = impl.Get奔跑速度;
        Get朝向 = impl.Get朝向;
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
        m_移动速度 = m_奔跑 ? Math.Sign(m_移动) * 奔跑速度 : m_移动;
    }
    public void On奔跑(InputAction.CallbackContext context)
    {
        m_奔跑 = context.ReadValue<bool>();
    }
    public void On翻滚(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        确定方向();
        m_tryDo.Push(CanDo.翻滚);
    }
    public void On飞扑(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        确定方向();
        m_tryDo.Push(CanDo.飞扑);
    }
    public void On拔刀(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        确定方向();
        m_tryDo.Push(m_移动 == 0 ? CanDo.拔刀 : CanDo.拔刀斩);
    }
    public void On气刃斩(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        确定方向();
        m_tryDo.Push(CanDo.气刃斩);
    }

    // private getter
    NewActions IAs => GetNewActions();
    NewActions.纳刀SActions 纳刀A => IAs.纳刀S;
    InputAction 飞扑IA => 纳刀A.飞扑;
    float 奔跑速度 => Get奔跑速度();
    PN1 朝向 => Get朝向();

    // private
    readonly CanDoBuffer m_tryDo = new();
    Func<NewActions> GetNewActions;
    Func<float> Get奔跑速度;
    Func<PN1> Get朝向;
    float m_移动;
    bool m_奔跑;

    void 确定方向()
    {
        m_方向 = m_移动 == 0 ? 朝向 : m_移动;
    }

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
}
public class 纳刀MActionsImpl : NewActions.I纳刀MActions
{
    public float m_移动速度;

    public void Register(IActionsImpl impl)
    {
        GetNewActions = impl.GetNewActions;
        纳刀A.SetCallbacks(this);
    }
    public void On移动(InputAction.CallbackContext context)
    {
        m_移动速度 = context.ReadValue<float>();
    }

    NewActions IAs => GetNewActions();
    NewActions.纳刀MActions 纳刀A => IAs.纳刀M;

    Func<NewActions> GetNewActions;
}
public class 拔刀SActionsImpl : NewActions.I拔刀SActions
{
    // public getter
    public ICanDoBuffer TryDo => m_tryDo;
    public float m_移动速度;
    public PN1 m_方向;

    public enum CanDo
    {
        空, 翻滚, 纳刀, 戳, 气刃斩
    }
    public interface ICanDoBuffer
    {
        public CanDo Peak();
        public CanDo Pop();
    }

    public void Register(IActionsImpl impl)
    {
        GetNewActions = impl.GetNewActions;
        Get朝向 = impl.Get朝向;
        拔刀A.SetCallbacks(this);
    }
    public void On移动(InputAction.CallbackContext context)
    {
        m_移动速度 = context.ReadValue<float>();
    }
    public void On翻滚(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        确定方向();
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

    // private getter
    NewActions IAs => GetNewActions();
    NewActions.拔刀SActions 拔刀A => IAs.拔刀S;
    PN1 朝向 => Get朝向();

    readonly CanDoBuffer m_tryDo = new();
    Func<NewActions> GetNewActions;
    Func<PN1> Get朝向;

    void 确定方向()
    {
        m_方向 = m_移动速度 == 0 ? 朝向 : m_移动速度;
    }

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
}
public class 攻击SActionsImpl : NewActions.I攻击SActions
{
    // public getter
    public ICanDoBuffer TryDo => m_tryDo;
    public PN1 方向 { get; set; }

    public enum CanDo
    {
        空, 翻滚, 纳刀, 戳, 气刃斩, 看破斩, 特殊纳刀
    }
    public interface ICanDoBuffer
    {
        public CanDo Peak();
        public CanDo Pop();
    }

    public void Register(IActionsImpl impl)
    {
        GetNewActions = impl.GetNewActions;
        Get朝向 = impl.Get朝向;
        攻击A.SetCallbacks(this);
    }
    public void On方向(InputAction.CallbackContext context)
    {
        m_方向 = context.ReadValue<float>();
    }
    public void On翻滚(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        确定方向();
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

    // private getter
    NewActions IAs => GetNewActions();
    NewActions.攻击SActions 攻击A => IAs.攻击S;
    PN1 朝向 => Get朝向();

    readonly CanDoBuffer m_tryDo = new();
    Func<NewActions> GetNewActions;
    Func<PN1> Get朝向;
    float m_方向;

    void 确定方向()
    {
        方向 = m_方向 == 0 ? 朝向 : m_方向;
    }

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
}
public class 特殊纳刀SActionsImpl : NewActions.I特殊纳刀SActions
{
    // public getter
    public ICanDoBuffer TryDo => m_tryDo;
    public PN1 方向 { get; set; }

    public enum CanDo
    {
        空, 翻滚, 居合拔刀斩, 居合拔刀气刃斩
    }
    public interface ICanDoBuffer
    {
        public CanDo Peak();
        public CanDo Pop();
    }

    public void Register(IActionsImpl impl)
    {
        GetNewActions = impl.GetNewActions;
        Get朝向 = impl.Get朝向;
        特殊纳刀A.SetCallbacks(this);
    }
    public void On翻滚(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        确定方向();
        m_tryDo.Push(CanDo.翻滚);
    }
    public void On居合拔刀斩(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        确定方向();
        m_tryDo.Push(CanDo.居合拔刀斩);
    }
    public void On居合拔刀气刃斩(InputAction.CallbackContext context)
    {
        if (!context.ReadValue<bool>()) return;
        m_tryDo.Push(CanDo.居合拔刀气刃斩);
    }

    // private getter
    NewActions IAs => GetNewActions();
    NewActions.特殊纳刀SActions 特殊纳刀A => IAs.特殊纳刀S;
    PN1 朝向 => Get朝向();

    readonly CanDoBuffer m_tryDo = new();
    Func<NewActions> GetNewActions;
    Func<PN1> Get朝向;
    float m_方向;

    void 确定方向()
    {
        方向 = m_方向 == 0 ? 朝向 : m_方向;
    }

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
}


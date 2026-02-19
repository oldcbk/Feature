using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class KatanaHero : MonoBehaviour
{
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

    [Serializable]
    public record Operation // 用record自动实现值相等性
    {
        public string m_name;
        public Operation(string name = "no name") { m_name = name; }
    }

    public List<Operation> m_operations;
    public void InitOperations()
    {
        m_operations = new() {
            new("Y"),
            new("X"),
            new("RT"),
            new("RT+X"),
            new("RT+Y"),
        };
    }

    public class OperationBuffer
    {
        public float m_timestamp;
        Operation m_operation;

        public void Push(Operation operation)
        {
            m_operation = operation;
            m_timestamp = Time.time;
        }

        public Operation Peak()
        {
            return m_operation;
        }

        public Operation Pop()
        {
            var temp = m_operation;
            m_operation = null;
            return temp;
        }
    }
    public OperationBuffer m_operationBuffer;


    public class StateMachine
    {
        public MonoBehaviour MB => m_MB;

        readonly MonoBehaviour m_MB;
        State m_currentState;

        public StateMachine(MonoBehaviour monoBehaviour)
        {
            m_MB = monoBehaviour;
        }

        public void ChangeState(State state)
        {
            m_currentState.OnExit();
            m_currentState = state;
            m_currentState.OnEnter();
        }
    }

    public class State
    {
        public StateMachine SM => m_stateMachine;
        protected MonoBehaviour MB => SM.MB;
        protected KatanaHero KH => m_katanaHero;

        public string m_name;

        readonly StateMachine m_stateMachine;
        readonly KatanaHero m_katanaHero;

        public State(StateMachine stateMachine, string name)
        {
            m_stateMachine = stateMachine;
            m_katanaHero = MB as KatanaHero;
            m_name = name;
        }

        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }
    }

    State m_ready;

    public enum LogicalMove // 这里不体现优先级
    {
        空,
        行走,
        // --纳刀S--
        奔跑,
        纳刀翻滚M,
        飞扑M,
        拔刀M,
        拔刀斩S,
        // --拔刀S--
        纳刀M,
        拔刀翻滚M,
        戳S,
        气刃斩1S,
        气刃斩2S,
        气刃斩3S,
        气刃斩4S,
        看破斩S,
        特殊纳刀S,
        居合拔刀斩S,
        居合拔刀气刃斩S
    }
    public record InputMovePair
    {
        public InputAction m_physicalInput;
        public LogicalMove m_logicalMove;

        public InputMovePair(InputAction physicalInput = null, LogicalMove logicalMove = 0)
        {
            m_physicalInput = physicalInput;
            m_logicalMove = logicalMove;
        }
    }


    public class 纳刀S : State
    {
        List<InputMovePair> m_inputMovePairs = new(7) {
            new(null,LogicalMove.纳刀翻滚M),
            new(null,LogicalMove.飞扑M),
            new(null,LogicalMove.拔刀M),
            new(null,LogicalMove.拔刀斩S),
            new(null,LogicalMove.气刃斩1S),
            new(null,LogicalMove.奔跑),
            new(null,LogicalMove.行走),
        };

        public void GenBuffer(List<InputMovePair> inputMovePairs)
        {

        }

        public class Buffer
        {
            List<InputMovePair> m_inputMovePairs;
        }

        void foo(LogicalMove logicalMove)
        {

        }

        public void GenLogicalMove()
        {

        }

        public 纳刀S(StateMachine stateMachine, string name) : base(stateMachine, name)
        {
        }
    }


    public class Poke : State
    {
        List<Operation> Operations => KH.m_operations;
        Operation OperationCached => KH.m_operationBuffer.Peak();
        State ReadyState => KH.m_ready;
        State PokeState;

        Timer m_move;
        Timer m_sustain;
        bool m_isMoving;

        public Poke(StateMachine stateMachine, string name) : base(stateMachine, name)
        {
            m_move = new(MB, .4f);
            m_move.OnTimeUp += OnMoveTimeUp;
            m_sustain = new(MB, 1);
            m_sustain.OnTimeUp += OnSustainTimeUp;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            m_move.Start();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (OperationCached == Operations[0])
            {
                SM.ChangeState(PokeState);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            m_move.Stop();
            m_sustain.Stop();
        }

        void OnMoveTimeUp()
        {
            m_sustain.Start();
        }

        void OnSustainTimeUp()
        {
            SM.ChangeState(ReadyState);
        }
    }

    private void Start()
    {
        InitOperations();
    }
}

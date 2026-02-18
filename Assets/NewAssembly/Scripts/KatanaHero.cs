using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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

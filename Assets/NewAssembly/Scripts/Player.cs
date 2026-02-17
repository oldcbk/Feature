using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player : MonoBehaviour, InputActions.IPlayerActions
{
    public bool IsOnGround => m_groundProbe.m_hit;

    #region Components
    public Rigidbody2D m_rigidbody;
    public WallChecker m_groundProbe;
    #endregion

    public float m_horizontalVelocityFactor;
    public float m_jumpForce;
    public GravityFactor m_gravityFactor;

    public int m_maxJumpCnt;
    public int m_remainJumpCnt;

    #region StateMachine & States
    public StateMachine m_stateMachine;
    public GroundState m_groundState;
    public AirState m_airState;
    void InitStateMachine()
    {
        m_stateMachine = new();
        m_groundState = new(m_stateMachine, this);
        m_airState = new(m_stateMachine, this);
        m_stateMachine.Init(m_groundState);
    }
    #endregion

    [Header("dbg")]
    [SerializeField]
    private float m_horizontalPilotPoint;

    private InputActions m_inputActions;

    void Start()
    {
        InitStateMachine();
        m_inputActions = new();
        m_inputActions.Player.AddCallbacks(this);
        m_inputActions.Enable();
    }

    void Update()
    {
        m_stateMachine.OnUpdate();
    }

    private void FixedUpdate()
    {
        m_gravityFactor.OnFixedUpdate();
        m_rigidbody.velocity = new Vector2(m_horizontalPilotPoint * m_horizontalVelocityFactor, m_rigidbody.velocity.y);
    }


    public void OnLeft(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                {
                    m_horizontalPilotPoint -= 1;
                    break;
                }
            case InputActionPhase.Canceled:
                {
                    m_horizontalPilotPoint += 1;
                    break;
                }
        }
    }

    public void OnRight(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                {
                    m_horizontalPilotPoint += 1;
                    break;
                }
            case InputActionPhase.Canceled:
                {
                    m_horizontalPilotPoint -= 1;
                    break;
                }
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                {
                    if (m_remainJumpCnt <= 0) break;
                    m_remainJumpCnt--;
                    m_rigidbody.velocity = new Vector2(m_rigidbody.velocity.x, m_jumpForce);
                    m_gravityFactor.Apply(m_rigidbody, this);
                    break;
                }
            case InputActionPhase.Canceled:
                {
                    m_gravityFactor.Recover();
                    break;
                }
        }
    }

    public void OnUp(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnDown(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
}

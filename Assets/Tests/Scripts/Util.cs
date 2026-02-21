using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

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
public class Timer
{
    public bool IsRunning => m_isRunning;
    public event Action OnTimeUp;

    /// <param name="duration">seconds</param>
    public Timer(MonoBehaviour monoBehaviour = null, float duration = 0)
    {
        m_MB = monoBehaviour;
        m_duration = duration;
    }
    /// <param name="duration">seconds</param>
    public void Set(MonoBehaviour monoBehaviour = null, float duration = 0)
    {
        Assert.IsFalse(m_isRunning);
        m_MB = monoBehaviour;
        m_duration = duration;
    }
    /// <param name="duration">seconds</param>
    public void Set(float duration = 0) { m_duration = duration; }

    public void Start()
    {
        m_isRunning = true;
        m_coroutine = m_MB.StartCoroutine(TimerCoroutine());
    }

    public void Stop()
    {
        m_MB.StopCoroutine(m_coroutine);
        m_isRunning = false;
    }

    MonoBehaviour m_MB;
    float m_duration;

    bool m_isRunning;
    Coroutine m_coroutine;

    IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(m_duration);
        m_isRunning = false;
        OnTimeUp?.Invoke();
    }
}
public class Timer2
{
    public bool IsRunning => m_isRunning;
    public event Action OnTimeUp;

    public void Set(float duration) { m_duration = duration; }
    public void Start()
    {
        m_isRunning = true;
        m_elapsed = 0;
    }
    public void Tick(float delta)
    {
        if (!m_isRunning) return;
        m_elapsed += delta;
        if (m_elapsed < m_duration) return;
        OnTimeUp?.Invoke();
    }
    public void Stop()
    {
        m_isRunning = false;
    }

    float m_duration;
    float m_elapsed;
    bool m_isRunning;
}
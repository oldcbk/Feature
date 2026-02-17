using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class GravityFactor
{
    public float m_factor;
    public float m_duration;

    Rigidbody2D m_RB;
    MonoBehaviour m_MB;
    float m_gravityOld;
    Coroutine m_coroutine;

    public GravityFactor(float factor = 1, float duration = 1)
    {
        m_factor = factor;
        m_duration = duration;
    }

    public void Apply(Rigidbody2D rigidbody2D, MonoBehaviour MB)
    {
        Assert.IsTrue(m_RB == null);
        if (rigidbody2D == null || MB == null) return;
        m_RB = rigidbody2D;
        m_MB = MB;
        m_gravityOld = m_RB.gravityScale;
        m_RB.gravityScale *= m_factor;
        m_coroutine = m_MB.StartCoroutine(TimerCoroutine());
    }

    public void Recover()
    {
        if (m_RB == null) return;
        m_RB.gravityScale = m_gravityOld;
        m_RB = null;
        m_MB.StopCoroutine(m_coroutine);
    }

    public void OnFixedUpdate()
    {
        if (m_RB == null) return;
        if (m_RB.velocity.y <= 0) Recover();
    }

    IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(m_duration);
        Recover();
    }
}

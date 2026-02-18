using UnityEngine;

public class UFO : MonoBehaviour
{
    public Rigidbody2D m_rigidbody2D;

    [Tooltip("degree")]
    public float m_angleToX;
    public float m_speed;
    bool m_isMoving;
    Vector2 m_originalPosition;

    Vector3 LaunchDir => Quaternion.Euler(0, 0, m_angleToX) * Vector3.right;

    private void Start()
    {
        m_originalPosition = transform.position;
    }

    [Button(nameof(Launch))]
    public string LaunchButton;
    public void Launch()
    {
        if (m_isMoving) Back();
        m_isMoving = true;
        m_originalPosition = transform.position;
        m_rigidbody2D.velocity = LaunchDir * m_speed;
    }

    [Button(nameof(Back))]
    public string BackButton;
    public void Back()
    {
        m_rigidbody2D.velocity = Vector3.zero;
        transform.position = m_originalPosition;
        m_isMoving = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, LaunchDir);
    }
}

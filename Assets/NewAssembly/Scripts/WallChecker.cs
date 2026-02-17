using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallChecker : MonoBehaviour
{
    public LayerMask m_wallLayer;
    public float m_length;
    public bool m_hit;

    private RaycastHit2D m_hitCache;

    private string GetName(GameObject gameObject)
    {
        List<GameObject> gameObjects = new()
        {
            gameObject
        };
        while (gameObject.transform.parent != null)
        {
            gameObject = gameObject.transform.parent.gameObject;
            gameObjects.Add(gameObject);
        }
        string name = "";
        foreach (var go in gameObjects.Reverse<GameObject>())
        {
            name += go.name;
            name += ".";
        }
        return name;
    }

    void Update()
    {
        var hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector3.right), m_length, m_wallLayer);
        m_hit = hit.collider != null;
        if (m_hit && hit.collider != m_hitCache.collider)
        {
            print($"WallChecker {GetName(gameObject)} hit {GetName(hit.collider.gameObject)}");
        }
        m_hitCache = hit;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.TransformPoint(m_length * Vector3.right));
    }
}

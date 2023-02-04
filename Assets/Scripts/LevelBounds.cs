using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBounds : MonoBehaviour
{
    [Header("Bounds")]
    public Vector2 bounds = new Vector2(70f, 70f);

    public float Right => transform.position.x + bounds.x * 0.5f;
    public float Left => transform.position.x - bounds.x * 0.5f;
    public float Top => transform.position.y + bounds.y * 0.5f;
    public float Bottom => transform.position.y - bounds.y * 0.5f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, bounds);
    }
}

using UnityEngine;

public class DartboardGizmo : MonoBehaviour
{
    public float boardRadius = 1.5f;

    void OnDrawGizmos()
    {
        float[] ratios = { 6.35f/170f, 15.9f/170f, 99f/170f, 107f/170f, 162f/170f, 1f };
        Gizmos.color = Color.yellow;
        foreach (float r in ratios) DrawCircle(r * boardRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * boardRadius);
    }

    void DrawCircle(float radius)
    {
        Vector3 prev = transform.position + transform.right * radius;
        for (int i = 1; i <= 64; i++)
        {
            float a = i * Mathf.PI * 2f / 64f;
            Vector3 next = transform.position + (transform.right * Mathf.Cos(a) + transform.up * Mathf.Sin(a)) * radius;
            Gizmos.DrawLine(prev, next);
            prev = next;
        }
    }
}
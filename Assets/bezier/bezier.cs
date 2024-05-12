using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bezier : MonoBehaviour
{
    public Transform p0, p1, p2;

    public int resolution = 50;
    private Vector3[] positions;
    Vector3[] points;
    private void Awake()
    {
        
    }
    private void Start()
    {
    }
    private void OnDrawGizmos()
    {
        DrawAnchers();
        DrawBezierCurve();
        DrawBezierPoints();
    }

    void DrawAnchers()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(p0.position, 0.1f);
        Gizmos.DrawSphere(p1.position, 0.1f);
        Gizmos.DrawSphere(p2.position, 0.1f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(p0.position, p1.position);
        Gizmos.DrawLine(p1.position, p2.position);
    }
        void DrawBezierCurve()
    {
        positions = new Vector3[50];
        points = new Vector3[50];
        Gizmos.color = Color.red;
        Vector3 start = p0.position;
        for (int i = 1; i <= resolution; i ++)
        {
            float t = i / (float)resolution;
            positions[i-1] = CalculateBezierPoint(t, p0.position, p1.position, p2.position);
            points[i - 1] = positions[i - 1];
            Gizmos.DrawLine(start, positions[i - 1]);
            start = positions[i - 1];
        }
    }

    void DrawBezierPoints()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.DrawSphere(points[i], 0.02f);
        }

    }
    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        // The formula for a quadratic Bezier curve
        return Mathf.Pow(1 - t, 2) * p0 + 2 * (1 - t) * t * p1 + Mathf.Pow(t, 2) * p2;
    }
}

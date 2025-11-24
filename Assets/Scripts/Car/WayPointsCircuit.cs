using UnityEngine;
using System.Collections.Generic;

public class WaypointCircuit : MonoBehaviour
{
    
    public List<Transform> waypoints = new();

    void OnDrawGizmos()
    {
        // Visualisasi jalur di Scene View agar mudah ditata
        if (transform.childCount > 0)
        {
            waypoints.Clear();
            foreach (Transform t in transform) waypoints.Add(t);

            Gizmos.color = Color.cyan;
            for (int i = 0; i < waypoints.Count; i++)
            {
                Vector3 current = waypoints[i].position;
                Vector3 next = waypoints[(i + 1) % waypoints.Count].position;
                Gizmos.DrawLine(current, next);
                Gizmos.DrawSphere(current, 0.5f);
            }
        }
    }
}
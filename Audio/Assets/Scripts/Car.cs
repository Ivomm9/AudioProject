using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent; // Referencia al NavMeshAgent
    public List<Transform> waypoints; // Lista de waypoints
    public float waypointThreshold = 1.0f; // Distancia m�nima para considerar que lleg� al waypoint

    private int currentWaypointIndex = 0;

    public int firstWaypointIndex = 0;

    void Start()
    {
        if (waypoints.Count > 0)
        {
            // Configura el primer destino
            agent.SetDestination(waypoints[firstWaypointIndex + 1].position);
            agent.Warp(waypoints[firstWaypointIndex].position);
        }
    }

    void Update()
    {
        if (waypoints.Count == 0) return;

        // Comprueba si el agente est� cerca del waypoint actual
        if (!agent.pathPending && agent.remainingDistance <= waypointThreshold)
        {
            // Avanza al siguiente waypoint
            currentWaypointIndex++;

            // Si llega al final, teletransporta al primero
            if (currentWaypointIndex >= waypoints.Count)
            {
                currentWaypointIndex = 0;
                agent.Warp(waypoints[currentWaypointIndex].position); // Teletransporta al primer waypoint
            }

            // Configura el siguiente destino
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }
}

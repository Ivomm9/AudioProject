using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Surface : MonoBehaviour
{
    [SerializeField] private Surfaces m_Surface;
    [SerializeField] private bool m_Ontrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (m_Ontrigger && !other.CompareTag("Player")) return;


        FootstepCollection.surface = m_Surface;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!m_Ontrigger && !collision.gameObject.CompareTag("Player")) return;


        FootstepCollection.surface = m_Surface;
    }
}

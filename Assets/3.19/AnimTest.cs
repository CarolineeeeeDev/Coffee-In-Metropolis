using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AnimTest : MonoBehaviour
{
    Animator m_Aanimator;
    NavMeshAgent meshAgent;
    void Start()
    {
        m_Aanimator = GetComponent<Animator>();
        meshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Aanimator.SetBool ("walk", (meshAgent.destination - transform.position).magnitude > 0.5);
    }
}

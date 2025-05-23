using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    [SerializeField] private float wanderRadius = 10f;
    [SerializeField] private float wanderInterval = 3f;

    private NavMeshAgent _agent;
    private float _timer;
    private void Awake()
    {   
        _agent = GetComponent<NavMeshAgent>();
        _timer = wanderInterval;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= wanderInterval)
        {
            Vector3 newPos;
            if (GetRandomPoint(transform.position, wanderRadius, out newPos))
            {
                _agent.SetDestination(newPos);
            }

            _timer = 0f;
        }
    }

    bool GetRandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            randomPoint.y = center.y;

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = center;
        return false;
    }
}

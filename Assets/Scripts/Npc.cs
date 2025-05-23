using System;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class NPC : MonoBehaviour
{
    private NavMeshAgent _agent;
    
    private float _wanderRadius = 10f;
    private float _interval = 5f;

    private float _timer = 0;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _timer = _interval;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        
        if (_timer >= _interval)
        {
            if (GetRandomPoint(transform.position, _wanderRadius, out Vector3 destination))
            {
                NavMeshPath path = new NavMeshPath();
                if (_agent.CalculatePath(destination, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    _agent.SetPath(path);
                }
            }
            _timer = 0f;
        }
    }

    bool GetRandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPos = center + Random.insideUnitSphere * range;
            randomPos.y = center.y;

            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = center;
        return false;
    }
}

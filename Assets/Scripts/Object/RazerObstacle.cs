using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazerObstacle : MonoBehaviour
{
    [SerializeField] private LineRenderer[] renderers;
    [SerializeField] private float forcePower;

    private void Update()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            var line = renderers[i];
            
            Vector3 startPos = line.transform.TransformPoint(line.GetPosition(0));
            Vector3 endPos = line.transform.TransformPoint(line.GetPosition(1));
            
            Vector3 dir = (endPos - startPos).normalized;
            float dist = Vector3.Distance(startPos, endPos);
            
          
            if (Physics.Raycast(startPos, dir, out RaycastHit hit, dist))
            {
                if (hit.collider.TryGetComponent(out Rigidbody rigid))
                {
                    rigid.velocity =  Vector3.zero;
                    
                    rigid.AddForce(transform.forward.normalized * forcePower, ForceMode.Impulse);
                }
            }
        }
    }
  
}

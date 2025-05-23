using System;
using System.Collections;
using UnityEngine;

public class RazerObstacle : MonoBehaviour
{
    [SerializeField] private LineRenderer[] renderers;
    [SerializeField] private float forcePower;
    [SerializeField] private float damage;
    

    private void Start()
    {
        StartCoroutine(RazerCorutine());
    }

    IEnumerator RazerCorutine()
    {
        while (true)
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
                    
                        Vector3 force = (rigid.transform.up + rigid.transform.forward * -2).normalized * forcePower;
                    
                        rigid.AddForce(force, ForceMode.Impulse);

                        if (rigid.TryGetComponent(out PlayerConditionHandler conditionHandler))
                        {
                            conditionHandler.ChangeHelath(damage * -1);
                        }
                        
                        yield return new WaitForSeconds(0.5f);
                    }
                }
            }

            yield return null;
        }
    }
  
}

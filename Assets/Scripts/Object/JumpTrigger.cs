using System;
using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    
    [SerializeField] private float jumpForce;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.TryGetComponent(out Rigidbody rigid))
        {
            Vector3 velocity = rigid.velocity;
            velocity.y = 0;
            rigid.velocity = velocity;
            
            rigid.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
}

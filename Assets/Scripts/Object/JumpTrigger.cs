using System;
using UnityEngine;

public class JumpTrigger : MonoBehaviour
{
    [SerializeField] private float jumpForce;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Rigidbody rigid))
        {
            Vector3 velocity = rigid.velocity;
            velocity.y = 0;
            rigid.velocity = velocity;
            
            rigid.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

}

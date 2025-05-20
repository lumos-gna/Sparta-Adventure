using UnityEngine;

public class JumpPlatform : MonoBehaviour, IInteractable
{

    public string KeyText => "E";
    public string DescriptionText  => "Use";

    
    [SerializeField] private LayerMask targetLayer;
    
    [SerializeField] private float jumpForce;
    
    [SerializeField] private float targetTriggerVelocity;


    private void OnTriggerEnter(Collider other)
    {
        if ((targetLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            if (other.TryGetComponent(out Rigidbody rigid))
            {
                if (rigid.velocity.y < targetTriggerVelocity)
                {
                    Vector3 velocity = rigid.velocity;
                    velocity.y = 0;
                    rigid.velocity = velocity;
            
                    rigid.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                }
            }
        }
    }


    public void Interact(GameObject source)
    {
    }
}

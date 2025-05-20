using UnityEngine;

public class ClimbTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayer;
    
    private void OnTriggerEnter(Collider other)
    {
        if ((targetLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            if (other.TryGetComponent(out PlayerMovementHandler playerMovement))
            {
                playerMovement.SetClimb(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((targetLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            if (other.TryGetComponent(out PlayerMovementHandler playerMovement))
            {
                playerMovement.SetClimb(false);
            }
        }
    }
}

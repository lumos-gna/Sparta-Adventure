using UnityEngine;

public class ClimbTrigger : MonoBehaviour, IInteractable
{
    public bool IsInteractable { get; }
    public Vector3 InteractInfoPos => transform.position;
    

    [SerializeField] private BoolEventChannelSO toggleClimbChannel;
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerInteractHandler interactHandler))
        {
            toggleClimbChannel.Raise(false);
            
            interactHandler.EndInteract();
        }
    }

    public void Interact(GameObject source)
    {
        source.transform.rotation = Quaternion.LookRotation(-transform.forward);

        
        Vector3 targetPosition = transform.position - transform.forward;

        targetPosition.y = source.transform.position.y;

        source.transform.position = targetPosition;
        
        toggleClimbChannel.Raise(true);
    }
}

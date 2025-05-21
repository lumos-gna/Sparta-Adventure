using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float detectDistance;
    
   
    [Space(5f)]
    [SerializeField] private InteractableEventChannelSO toggleDetectedInteractableChannel;


    private bool _isInteracting;
    
    private IInteractable _curInteractTarget;
    
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_isInteracting) return;
        
        
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);

        float detectDist = Mathf.Abs(transform.position.z - _camera.transform.position.z) + detectDistance;
        
        if (Physics.Raycast(ray, out RaycastHit hit, detectDist, targetLayer))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                if (_curInteractTarget != interactable)
                {
                    _curInteractTarget = interactable;

                    toggleDetectedInteractableChannel.Raise(_curInteractTarget);
                }
            }
        }
        else
        {
            if (_curInteractTarget != null)
            {
                _curInteractTarget = null;

                toggleDetectedInteractableChannel.Raise(null);
            }
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (!_isInteracting && _curInteractTarget != null)
            {
                _isInteracting = true;

                _curInteractTarget.Interact(gameObject);
            
                toggleDetectedInteractableChannel.Raise(null);
            }
        }
    }

    public void EndInteract()
    {
        _isInteracting = false;

        _curInteractTarget = null;
    }
        
}

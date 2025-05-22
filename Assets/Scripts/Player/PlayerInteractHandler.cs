using UnityEngine;

public class PlayerInteractHandler : MonoBehaviour
{
    public IInteractable CurTarget { get; private set; }

    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float detectDistance;
   

    private bool _isInteracting;
    
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }


    public void Interact()
    {
        if (!_isInteracting && CurTarget != null)
        {
            _isInteracting = true;

            CurTarget.Interact(gameObject);
        }
    }
    
    public void EndInteract()
    {
        _isInteracting = false;

        CurTarget = null;
    }


    public void DetectInteractable()
    {
        if (_isInteracting) return;
        
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);

        float detectDist = Mathf.Abs(transform.position.z - _camera.transform.position.z) + detectDistance;
        
        if (Physics.Raycast(ray, out RaycastHit hit, detectDist))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                CurTarget = interactable;
                
                return;
            }
        }

        CurTarget = null;
    }

  
}

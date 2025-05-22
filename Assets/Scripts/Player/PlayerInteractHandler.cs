using UnityEngine;

public class PlayerInteractHandler : MonoBehaviour
{
    public IInteractable CurTarget { get; private set; }

    [SerializeField] private float detectDistance;


    private PlayerController _controller;
    
    private Camera _camera;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
        
        _camera = Camera.main;
    }


    public void Interact()
    {
        if (CurTarget != null)
        {
            CurTarget.Interact(_controller);

            CurTarget = null;
        }   
    }
    

    public void DetectInteractable()
    {
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

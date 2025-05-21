using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraHandler : MonoBehaviour
{
    [SerializeField] private Transform cameraContainer;

    
    [Space(10f)] 
    [Header("Setting")] 
    [SerializeField] private float camHeight;
    
    [Space(10f)]
    [SerializeField] private float minLookAngleX;
    [SerializeField] private float maxLookAngleX;
    
    [Space(10f)]
    [SerializeField] private float minLookDist;
    [SerializeField] private float maxLookDist;
    
    [Space(10f)]
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float zoomSensitivity;


    private float _camDist = 3f;
    private float _zoomDelta;
    private float _camObstacleDist;
    
    private Vector3 PlayerPos =>  transform.position + Vector3.up * camHeight;

    private Vector2 _lookInputAngle;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;                  
    }

    private void FixedUpdate()
    {
        UpdateObstacleDist();
    }

    private void LateUpdate()
    {
        UpdatePos();
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            var inputDelta = context.ReadValue<Vector2>();
            
            float angleX = _lookInputAngle.x + (inputDelta.y * lookSensitivity * -1f);
            float angleY = _lookInputAngle.y + (inputDelta.x * lookSensitivity);
        
            float clampAngleX = Mathf.Clamp(angleX, minLookAngleX, maxLookAngleX);
        
            _lookInputAngle = new Vector2(clampAngleX, angleY);
        }
    }

    public void OnZoomInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            var inputDelta = context.ReadValue<Vector2>();
            
            _camDist += inputDelta.y * zoomSensitivity * -1f;
        }
    }
    

    void UpdatePos()
    {
        cameraContainer.eulerAngles = _lookInputAngle;

        _camDist = Mathf.Clamp(_camDist, minLookDist, maxLookDist);

        float totalDist = _camDist - _camObstacleDist;
        
        Vector3 camPos = PlayerPos - cameraContainer.forward * totalDist;

        cameraContainer.position = camPos;
    }


    void UpdateObstacleDist()
    {
        Vector3 dir = cameraContainer.position - PlayerPos;
        
        Ray ray = new Ray(PlayerPos, dir.normalized);

        if (Physics.Raycast(ray, out RaycastHit hit, _camDist))
        {
            float hitDist = Vector3.Distance(PlayerPos, hit.point);
            
            _camObstacleDist = Mathf.Clamp(_camDist - hitDist, 0f, _camDist);
        }
        else
        {
            _camObstacleDist = 0;
        }
    }
}

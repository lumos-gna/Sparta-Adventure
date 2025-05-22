using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraHandler : MonoBehaviour
{
    [SerializeField] private Transform cameraContainer;

    [Space(10f)] 
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

    private Vector2 _lookInputAngle = Vector2.zero;

    private PlayerController _controller;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;                  
    }
 

    public void Zoom()
    {
        Vector2 inputDelta = _controller.ZoomInputDelta;

        _camDist += inputDelta.y * zoomSensitivity * -1f;
    }

    public void Handle()
    {
        Vector3 inputDelta = _controller.LookInputDelta;
        
        float angleX = _lookInputAngle.x + (inputDelta.y * lookSensitivity * -1f);
        float angleY = _lookInputAngle.y + (inputDelta.x * lookSensitivity);
        
        float clampAngleX = Mathf.Clamp(angleX, minLookAngleX, maxLookAngleX);
        
        _lookInputAngle = new Vector2(clampAngleX, angleY);
        
        cameraContainer.eulerAngles = _lookInputAngle;

        _camDist = Mathf.Clamp(_camDist, minLookDist, maxLookDist);

        float totalDist = _camDist - GetObstacleDist();
        
        
        Vector3 playerPos =  transform.position + Vector3.up * camHeight;
        
        Vector3 camPos = playerPos - cameraContainer.forward * totalDist;

        cameraContainer.position = camPos;
    }


    float GetObstacleDist()
    {
        Vector3 playerPos =  transform.position + Vector3.up * camHeight;
        
        Vector3 dir = cameraContainer.position - playerPos;
        
        Ray ray = new Ray(playerPos, dir.normalized);

        if (Physics.Raycast(ray, out RaycastHit hit, _camDist))
        {
            float hitDist = Vector3.Distance(playerPos, hit.point);
            
            return Mathf.Clamp(_camDist - hitDist, 0f, _camDist);
        }
        else
        {
            return 0;
        }
    }
}

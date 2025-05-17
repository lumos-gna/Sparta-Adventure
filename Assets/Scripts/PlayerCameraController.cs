using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera mainCamera;

    
    [Space(10f)] 
    [Header("Look")] 
    [SerializeField] private float minLookAngleX;
    [SerializeField] private float maxLookAngleX;
    [SerializeField] private float lookSensitivity;

    private Vector2 _lookInputAngle;
    
    
    [Space(10f)]
    [Header("Events")]
    [SerializeField] private Vector2EventChannelSO lookInputChannel;
    
    
    private void Start()
    {
        lookInputChannel.OnEventRasied += SetLookInputAngle;
    }
    
    private void LateUpdate()
    {
        Look();
    }

    private void OnDestroy()
    {
        lookInputChannel.OnEventRasied -= SetLookInputAngle;
    }
    
    void SetLookInputAngle(Vector2 mouseDelta)
    {
        float angleX = _lookInputAngle.x + (mouseDelta.y * lookSensitivity * -1f);
        float angleY = _lookInputAngle.y + (mouseDelta.x * lookSensitivity);
        
        float clampAngleX = Mathf.Clamp(angleX, minLookAngleX, maxLookAngleX);
        
        _lookInputAngle = new Vector2(clampAngleX, angleY);
    }

    void Look()
    {
        mainCamera.transform.localEulerAngles = _lookInputAngle;
    }
}

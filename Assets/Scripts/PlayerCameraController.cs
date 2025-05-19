using System;
using System.Collections;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform cameraContainer;

    
    [Space(10f)] 
    [Header("Look")] 
    [SerializeField] private float minLookAngleX;
    [SerializeField] private float maxLookAngleX;
    [SerializeField] private float lookSensitivity;

    private Vector2 _lookInputAngle;
    
    
    [Space(10f)]
    [Header("Zoom")] 
    [SerializeField] private float minZoomDist;
    [SerializeField] private float maxZoomDist;
    [SerializeField] private float zoomSensitivity;

    private float _camDistance = 3f;

    
    [Space(10f)]
    [Header("Events")]
    [SerializeField] private Vector2EventChannelSO lookInputChannel;
    [SerializeField] private Vector2EventChannelSO zoomInputChannel;

    
    
    private void Start()
    {
        lookInputChannel.OnEventRasied += SetLookInputAngle;
        zoomInputChannel.OnEventRasied += Zoom;
    }
    
    
    private void LateUpdate()
    {
        Move();
    }
    

    private void OnDestroy()
    {
        lookInputChannel.OnEventRasied -= SetLookInputAngle;
        zoomInputChannel.OnEventRasied -= Zoom;
    }
    
    
    
    void SetLookInputAngle(Vector2 inputDelta)
    {
        float angleX = _lookInputAngle.x + (inputDelta.y * lookSensitivity * -1f);
        float angleY = _lookInputAngle.y + (inputDelta.x * lookSensitivity);
        
        float clampAngleX = Mathf.Clamp(angleX, minLookAngleX, maxLookAngleX);
        
        _lookInputAngle = new Vector2(clampAngleX, angleY);
    }
    


    void Move()
    {
        cameraContainer.eulerAngles = _lookInputAngle;
     
        cameraContainer.position = transform.position - cameraContainer.forward * _camDistance;
    }
    
    

    void Zoom(Vector2 inputDelta)
    {
        _camDistance += inputDelta.y * zoomSensitivity * -1f;

        _camDistance = Mathf.Clamp(_camDistance, minZoomDist, maxZoomDist);
    }
}

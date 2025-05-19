using System;
using System.Collections;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform cameraContainer;

    
    [Space(10f)] 
    [Header("Setting")] 
    [SerializeField] private float minLookAngleX;
    [SerializeField] private float maxLookAngleX;
    
    [Space(10f)]
    [SerializeField] private float minLookDist;
    [SerializeField] private float maxLookDist;
    
    [Space(10f)]
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float zoomSensitivity;

    
    [Space(10f)]
    [Header("Events")]
    [SerializeField] private Vector2EventChannelSO lookInputChannel;
    [SerializeField] private Vector2EventChannelSO zoomInputChannel;

    
    private float _camDist = 3f;
    private float _zoomDelta;
    private float _camObstacleDist;

    private Vector2 _lookInputAngle;

    
    private void Start()
    {
        lookInputChannel.OnEventRaised += UpdateLookInputAngle;
        zoomInputChannel.OnEventRaised += Zoom;
    }


    private void LateUpdate()
    {
        UpdateObstacleDist();
        UpdatePos();
    }
    

    private void OnDestroy()
    {
        lookInputChannel.OnEventRaised -= UpdateLookInputAngle;
        zoomInputChannel.OnEventRaised -= Zoom;
    }
    
    
    void UpdateLookInputAngle(Vector2 inputDelta)
    {
        float angleX = _lookInputAngle.x + (inputDelta.y * lookSensitivity * -1f);
        float angleY = _lookInputAngle.y + (inputDelta.x * lookSensitivity);
        
        float clampAngleX = Mathf.Clamp(angleX, minLookAngleX, maxLookAngleX);
        
        _lookInputAngle = new Vector2(clampAngleX, angleY);
    }


    void UpdatePos()
    {
        cameraContainer.eulerAngles = _lookInputAngle;


        _camDist = Mathf.Clamp(_camDist, minLookDist, maxLookDist);

        float totalDist = _camDist - _camObstacleDist;
        
        cameraContainer.position = transform.position - cameraContainer.forward * totalDist;
    }


    void Zoom(Vector2 inputDelta) =>  _camDist += inputDelta.y * zoomSensitivity * -1f;
   

    void UpdateObstacleDist()
    {
        Vector3 dir = cameraContainer.position - transform.position;
        
        Ray ray = new Ray(transform.position, dir.normalized);

        if (Physics.Raycast(ray, out RaycastHit hit, _camDist))
        {
            float hitDist = Vector3.Distance(transform.position, hit.point);
            
            _camObstacleDist = Mathf.Clamp(_camDist - hitDist, 0f, _camDist);
        }
        else
        {
            _camObstacleDist = 0;
        }
    }
}

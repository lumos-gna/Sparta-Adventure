using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Camera mainCamera;
    
    [Space(10f)]
    [Header("Settings")]
    [SerializeField] private LayerMask detectLayer;
    [SerializeField] private float detectDistance;
    
    
    [Space(10f)]
    [Header("Events")]
    [SerializeField] private InteractableEventChannelSO enterInteractableChannel;
    [SerializeField] private InteractableEventChannelSO exitInteractableChannel;
    
    
    private IInteractable _curInteractTarget;
    
    private void FixedUpdate()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f,  Screen.height / 2f));

        if (Physics.Raycast(ray, out RaycastHit hit, detectDistance, detectLayer))
        {
            if (_curInteractTarget == null)
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    enterInteractableChannel.Raise(interactable);

                    _curInteractTarget = interactable;
                }
            }
        }
        else
        {
            if (_curInteractTarget != null)
            {
                exitInteractableChannel.Raise(_curInteractTarget);
                
                _curInteractTarget = null;
            }
        }
    }
    
}

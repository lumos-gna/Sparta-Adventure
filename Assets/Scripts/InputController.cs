using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerInput playerInput;

    [Space(10f)]
    [Header("Events")]
    [SerializeField] private Vector2EventChannelSO moveInputChannel;
    [SerializeField] private Vector2EventChannelSO lookInputChannel;
    [SerializeField] private Vector2EventChannelSO zoomInputChannel;
    [SerializeField] private VoidEventChannelSO jumpStartInputChannel;
    [SerializeField] private VoidEventChannelSO jumpEndInputChannel;

    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            moveInputChannel.Raise(context.ReadValue<Vector2>());
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            moveInputChannel.Raise(Vector2.zero);
        }
    }

    public void OnLookInput(InputAction.CallbackContext context) => lookInputChannel.Raise(context.ReadValue<Vector2>());
    public void OnZoomInput(InputAction.CallbackContext context) => zoomInputChannel.Raise(context.ReadValue<Vector2>());

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started : 
                jumpStartInputChannel.Raise(); break;
            case InputActionPhase.Canceled : 
                jumpEndInputChannel.Raise(); break;
        }
    }

}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool PressedJumpInput { get; private set; }
    
    public bool PressedInteractInput { get; private set; }
    
    public Vector2 LookInputDelta { get; private set; }
    public Vector2 ZoomInputDelta { get; private set; }
    public Vector2 MoveInputDelta { get; private set; }


    public Rigidbody Rigidbody { get; private set; }
    public PlayerCameraHandler CameraHandler { get; private set; }
    public PlayerConditionHandler ConditionHandler { get; private set; }
    public PlayerMovementHandler MovementHandler { get; private set; }
    public PlayerInteractHandler InteractHandler { get; private set; }
    

    private IPlayerState _curState;

    private Dictionary<IPlayerState.Type, IPlayerState> _stateDict;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        CameraHandler = GetComponent<PlayerCameraHandler>();
        ConditionHandler = GetComponent<PlayerConditionHandler>();
        MovementHandler = GetComponent<PlayerMovementHandler>();
        InteractHandler = GetComponent<PlayerInteractHandler>();

        _stateDict = new()
        {
            { IPlayerState.Type.Basic, new PlayerBasicState(this) },
            { IPlayerState.Type.Air, new PlayerAirState(this) },
            { IPlayerState.Type.Climb, new PlayerClimbState(this) }
        };

        ChangeState(IPlayerState.Type.Basic);
    }

    private void FixedUpdate()
    {
        _curState?.FixedUpdate();
    }

    private void Update()
    {
        _curState?.Update();
    }

    private void LateUpdate()
    {
        _curState?.LateUpdate();
        
        ResetInputState();
    }

    
    public void ChangeState(IPlayerState.Type type)
    {
        _curState?.Exit();

        _curState = _stateDict[type];
        
        _curState?.Enter();
    }
    
  
    
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            PressedJumpInput = true;
        }
    }
    
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            PressedInteractInput = true;
        }
    }
    
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed :
                MoveInputDelta = context.ReadValue<Vector2>();  break;
            
            case InputActionPhase.Canceled :
                MoveInputDelta = Vector3.zero; break;
        }
    }
    
    public void OnLookInput(InputAction.CallbackContext context) 
        => LookInputDelta = context.ReadValue<Vector2>();  
 
    public void OnZoomInput(InputAction.CallbackContext context)
        => ZoomInputDelta = context.ReadValue<Vector2>(); 


    private void ResetInputState()
    {
        PressedInteractInput = false;
        PressedJumpInput = false;
    }

}

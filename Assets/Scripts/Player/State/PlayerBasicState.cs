

using UnityEngine;

public class PlayerBasicState : IPlayerState
{
    private readonly PlayerController _controller;

    public PlayerBasicState(PlayerController controller)
    {
        _controller = controller;
    }
    
    public void Enter()
    {
    }

    public void FixedUpdate()
    {
        if (!_controller.MovementHandler.IsGrounded())
        {
            _controller.ChangeState(IPlayerState.Type.Air);

            return;
        }
        
        _controller.MovementHandler.BasicMove();
        
        _controller.MovementHandler.RotationToCamera();
        
        _controller.InteractHandler.DetectInteractable();
    }

    public void Update()
    {
        if (_controller.PressedJumpInput)
        {
            _controller.MovementHandler.Jump();
        }

        if (_controller.PressedInteractInput)
        {
            _controller.InteractHandler.Interact();
        }
    }

    public void LateUpdate()
    {
        _controller.CameraHandler.Handle();
        _controller.CameraHandler.Zoom();
    }

    public void Exit()
    {
    }
}
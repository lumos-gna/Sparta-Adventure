

using UnityEngine;

public class PlayerClimbState : IPlayerState
{
    private readonly PlayerController _controller;

    public PlayerClimbState(PlayerController controller)
    {
        _controller = controller;
    }

    
    public void Enter()
    {
        _controller.MovementHandler.InitClimbState(true);
    }

    public void FixedUpdate()
    {
        _controller.MovementHandler.ClimbMove();
    }

    public void Update()
    {
    }

    public void LateUpdate()
    {
        _controller.CameraHandler.Handle();
        _controller.CameraHandler.Zoom();
    }

    public void Exit()
    {
        _controller.MovementHandler.InitClimbState(false);
    }
}
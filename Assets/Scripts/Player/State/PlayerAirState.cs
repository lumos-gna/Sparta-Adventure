

public class PlayerAirState : IPlayerState
{
    
    private readonly PlayerController _controller;

    public PlayerAirState(PlayerController controller)
    {
        _controller = controller;
    }

    
    public void Enter()
    {
    }

    public void FixedUpdate()
    {
        _controller.MovementHandler.AirMove();
        
        _controller.MovementHandler.RotationToCamera();
    }

    public void Update()
    {
    }

    public void LateUpdate()
    {
        if (_controller.MovementHandler.IsGrounded())
        {
            _controller.ChangeState(IPlayerState.Type.Basic);

            return;
        }
        
        _controller.CameraHandler.Handle();
    }

    public void Exit()
    {
    }
}
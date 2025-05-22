
public interface IPlayerState
{
    public enum Type
    {
        Basic,
        Air,
        Climb
    }

    public void Enter();

    public void FixedUpdate();

    public void Update();
    
    public void LateUpdate();

    public void Exit();
}

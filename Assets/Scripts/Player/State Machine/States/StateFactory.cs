
public class StateFactory
{
    PlayerStateMachine _ctx;
    public PlayerBaseState _playerIdleState ;
    public PlayerBaseState _playerWalkState;
    public PlayerBaseState _playerJumpState ;
    public PlayerBaseState _playerInteractState ;
    public PlayerBaseState _playerSlideState ;
    public PlayerBaseState _playerCrouchState ;
    public PlayerBaseState _playerUnCrouchState;
    public StateFactory(PlayerStateMachine stateMachine)
    {
        _ctx = stateMachine;
        _playerIdleState = new PlayerIdleState(_ctx, this);
        _playerWalkState = new PlayerWalkState(_ctx, this);
        _playerJumpState = new PlayerJumpState(_ctx, this);
        _playerInteractState = new PlayerInteractState(_ctx, this);
        _playerSlideState = new PlayerSlideState(_ctx, this);
        _playerCrouchState = new PlayerCrouchState(_ctx, this);
        _playerUnCrouchState = new PlayerUnCrouchState(_ctx, this);
    }

    public PlayerBaseState Idle()
    {
        return _playerIdleState;
    }
    public PlayerBaseState Walk()
    {
        return _playerWalkState;
    }
    public PlayerBaseState Jump()
    {
        return _playerJumpState;
    }
    public PlayerBaseState Interact()
    {
        return _playerInteractState;
    }
    public PlayerBaseState Slide()
    {
        return _playerSlideState;
    }
    public PlayerBaseState Crouch()
    {
        return _playerCrouchState;
    }
    public PlayerBaseState UnCrouch()
    {
        return _playerUnCrouchState;
    }

}

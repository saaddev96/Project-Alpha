
public abstract class PlayerBaseState
{

    protected PlayerStateMachine ctx;
    protected StateFactory factory;
    protected bool isRoot = false;
    protected PlayerBaseState subState;
    public PlayerBaseState(PlayerStateMachine stateMachine, StateFactory stateFactory)
    {
        ctx = stateMachine;
        factory = stateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckState();
    public abstract void InitSubState();
    public void UpdateStates()
    {
        UpdateState();
        if (subState != null) { subState.UpdateStates(); }
        
    }
    public void SwitchState(PlayerBaseState newState)
    {
        ExitState();
        newState.EnterState();
        ctx.CurrentState = newState;
    }
    public void SetSubState(PlayerBaseState SubState)
    {
        //subState?.ExitState();
        subState = SubState;
        subState.EnterState();
    }
}

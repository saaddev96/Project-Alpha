using UnityEngine;
public class PlayerInteractState : PlayerBaseState
{
    InteractableBase interactableBase;
    public PlayerInteractState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {

    }

    public override void CheckState()
    {
        if (ctx.IsMoving && ctx.isGrounded && !ctx.Interacted)
        {
            SwitchState(factory.Walk());
        }
        if (ctx.IsJumping && ctx.CanJump&& !ctx.Interacted)
        {
            SwitchState(factory.Jump());
        }
        if (!ctx.IsInteracting)
        {
            SwitchState(factory.Idle());
        }

    }

    public override void EnterState()
    {
        ctx.PlayerState = PlayerState.Interacting;
        interactableBase = ctx.CurrentInteractable;
        interactableBase.OnInteract();
        interactableBase.OnInteracterExit();
        ctx.Interacted = false;
    }

    public override void ExitState()
    {
        interactableBase.OnInteractingExit();
    }

    public override void InitSubState()
    {

    }

    public override void UpdateState()
    {
        CheckState();
        if (interactableBase != null)
        {
            interactableBase.OnInteracting();
            ctx.AnimatorBrainPlay(eAnimation.Interact, ctx.Layer, false, false);
        }
    }



}

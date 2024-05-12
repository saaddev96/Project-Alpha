using UnityEngine;
public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
        
    }

    public override void CheckState()
    {
        if (ctx.IsMoving && ctx.isGrounded)
        {
            SwitchState(factory.Walk());
        }
        if(ctx.IsJumping && ctx.CanJump)
        {
            SwitchState(factory.Jump());
        }
        if (ctx.IsSliding)
        {
            SwitchState(factory.Slide());
        }
        if (ctx.AbleToInteract && ctx.CanInteract&&ctx.Interacted)
        {
            SwitchState(factory.Interact());
        }
    }

    public override void EnterState()
    {
        ctx.PlayerState = PlayerState.Idle;
        ctx.FPSControl();
    }

    public override void ExitState()
    {
       
    }

    public override void InitSubState()
    {

        if (ctx.IsCrouching && ctx.CanCrouch)
        {
            SetSubState(factory.Crouch());
        }
        else
        {
            SetSubState(factory.UnCrouch());
        }
    }

    public override void UpdateState()
    {
        CheckState();
        InitSubState();
        if (PlayerStateMachine.instance.IsAdsing)
        {
            ctx.AnimatorBrainPlay(eAnimation.Ads, ctx.Layer, false, false,0.08f);
        }
        else
        {
            ctx.AnimatorBrainPlay(eAnimation.Idle, ctx.Layer, false, false,0.08f);
        }
        ctx.CC.Move(ctx.moveDirection * Time.deltaTime);
    }




}

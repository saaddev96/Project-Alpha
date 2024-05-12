using UnityEngine;
public class PlayerWalkState : PlayerBaseState
{
    
    public PlayerWalkState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {

    }

    public override void CheckState()
    {
      
        if (ctx.IsJumping && ctx.CanJump)
        {
            SwitchState(factory.Jump());
        }
        if(ctx.IsSliding)
        {
            SwitchState(factory.Slide());
        }
        if (!ctx.IsMoving)
        {
            SwitchState(factory.Idle());
        }
        if(ctx.AbleToInteract && ctx.CanInteract && ctx.Interacted)
        {
            SwitchState(factory.Interact());
        }

    }

    public override void EnterState()
    {
        ctx.PlayerState = PlayerState.Walking;
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
        Move();
        ctx.CC.Move(ctx.moveDirection * Time.deltaTime);
    }

    void Move()
    {

        if (ctx.isGrounded)
        {
            ctx.FPSControl();
            if (ctx.IsSprinting && !ctx.IsCrouched&& !ctx.IsAdsing)
            {
                ctx.AnimatorBrainPlay(eAnimation.Idle, ctx.Layer, false, false);
            }
            else if (ctx.IsCrouched)
            {
                if (PlayerStateMachine.instance.IsAdsing)
                {
                    //ctx.AnimatorBrainPlay(eAnimation.AdsCrouch, ctx.Layer, false, false,0.08f);
                    ctx.AnimatorBrainPlay(eAnimation.Ads, ctx.Layer, false, false, 0.08f);
                }
                else
                {
                    //ctx.AnimatorBrainPlay(eAnimation.Crouch, ctx.Layer, false, false);
                    ctx.AnimatorBrainPlay(eAnimation.Idle, ctx.Layer, false, false, 0.08f);
                }
            }
            else
            {
                if (PlayerStateMachine.instance.IsAdsing)
                {
                    //ctx.AnimatorBrainPlay(eAnimation.AdsWalk, ctx.Layer, false, false, 0.08f);
                    ctx.AnimatorBrainPlay(eAnimation.Ads, ctx.Layer, false, false, 0.08f);
                }
                else
                {

                    //ctx.AnimatorBrainPlay(eAnimation.Walk, ctx.Layer, false, false);
                    ctx.AnimatorBrainPlay(eAnimation.Idle, ctx.Layer, false, false, 0.08f);
                }
            }
        }


    }
}

using UnityEngine;
using System;
public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {
    
    }

    public override void CheckState()
    {
        if (ctx.isGrounded)
        {
            SwitchState(factory.Idle());
        }
        if (ctx.IsSliding)
        {
            SwitchState(factory.Slide());
        }
    }

    public override void EnterState()
    {
        ctx.PlayerState = PlayerState.Jumping;
        jump();
       
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
        ctx.CC.Move(ctx.moveDirection * Time.deltaTime);
    }


    void jump()
    {
        if (!ctx.CheckAbovePlayer(out RaycastHit hit))
        {
            ctx.AnimatorBrainPlay(eAnimation.Jump, 0, false, false);
            ctx.moveDirection.y = Mathf.Sqrt(ctx.JumpHeight / 10 * -2f * ctx.GravityMultiplier);
        }
    }
    
   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlideState : PlayerBaseState
{

    public PlayerSlideState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {

    }
    public override void CheckState()
    {
        if (!ctx.IsSliding && ctx.isGrounded)
        {
            SwitchState(factory.Idle());
        }
    }

    public override void EnterState()
    {
        ctx.PlayerState = PlayerState.Sliding;
    
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
        Slide();
        ctx.CC.Move(ctx.moveDirection * Time.deltaTime);
    }

    void Slide()
    {
        Vector3 slopeDirection = Vector3.up - ctx.GroundCheckHitnormal * Vector3.Dot(Vector3.up, ctx.GroundCheckHitnormal);
        float slidespeed = ctx.SlopeSpeed + ctx.MoveSpeed + Time.deltaTime;
        ctx.moveDirection = slopeDirection*-slidespeed;
        ctx.moveDirection.y -= ctx.GroundCheckHitpoint.y;
        // ctx.moveDirection = new Vector3(ctx.GroundCheckHitnormal.x, -ctx.GroundCheckHitnormal.y, ctx.GroundCheckHitnormal.z) * ctx.SlopeSpeed;
    }
}

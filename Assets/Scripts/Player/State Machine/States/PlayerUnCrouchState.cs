using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnCrouchState : PlayerBaseState
{
    public PlayerUnCrouchState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
    {

    }
    public override void CheckState()
    {
      
    }

    public override void EnterState()
    {
     
    }

    public override void ExitState()
    {
      
    }

    public override void InitSubState()
    {

    }

    public override void UpdateState()
    {
        CheckState();
        UnCrouch();
    }

    void UnCrouch()
    {


        if (ctx.CC.height < ctx.PlayerHeight)
        {
            if (!ctx.CheckAbovePlayer(out RaycastHit hit))
            {
                ctx.IsCrouched = false;
                ctx.CC.height = Mathf.Lerp(ctx.CC.height, ctx.PlayerHeight, ctx.TimeToCrouch * Time.deltaTime);

            }
        }


    }

}

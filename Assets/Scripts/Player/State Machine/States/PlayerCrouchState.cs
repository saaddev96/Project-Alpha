using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PlayerCrouchState : PlayerBaseState
{
    public PlayerCrouchState(PlayerStateMachine ctx, StateFactory factory) : base(ctx, factory)
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
        Crouch();
    }

    void  Crouch()
    {
        
        if (ctx.CC.height > ctx.CrouchHeight)
        {
            ctx.IsCrouched = true;
            ctx.CC.height = Mathf.Lerp(ctx.CC.height, ctx.CrouchHeight, ctx.TimeToCrouch * Time.deltaTime);
        }
    }
    
}

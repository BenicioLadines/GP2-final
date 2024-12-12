using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : PlayerState
{
    public override void UpdateState()
    {
        if (!player.controlling)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            player.Jump();
        }

        if (Input.GetKeyDown(KeyCode.E) && Mathf.Abs(player.directionalInput.x) > 0)
        {
            player.Attack(PlayerControl.AttackType.forwardGround);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            player.Attack(PlayerControl.AttackType.neutralGround);
        }
        

        if(Input.GetKeyDown(KeyCode.S))
        {
            player.ChangeState(player.crouchState);
        }

    }

    public override void FixedUpdateState()
    {
        if (!player.controlling)
        {
            return;
        }

        if (Mathf.Abs(player.directionalInput.x) > 0)
        {
            if(player.directionalInput.x > 0 && !player.facingRight)
            {
                player.TurnAround();
            }
            
            if(player.directionalInput.x < 0 && player.facingRight)
            {
                player.TurnAround();
            }

            player.Walk();
        }
        else
        {
            player.GroundDecelerate();
        }

        if (!player.OnTheGround())
        {
            player.ChangeState(player.airState);
        }
    }
}

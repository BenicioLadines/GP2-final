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

        if(Input.GetButtonDown(player.jumpButton))
        {
            player.Jump();
        }

        if (Input.GetButtonDown(player.attackButton))
        {
            if(player.DirectionallyInputting())
            {
                switch (player.GetInputDirection())
                {
                    case 0:
                        player.Attack(PlayerControl.AttackType.upGround);
                        break;
                    case 1:
                        player.Attack(PlayerControl.AttackType.forwardGround);
                        break;
                    case -1:
                        player.Attack(PlayerControl.AttackType.forwardGround);
                        break;
                    default:
                        player.Attack(PlayerControl.AttackType.neutralGround); 
                        break;
                }
            }
            else
            {
                player.Attack(PlayerControl.AttackType.neutralGround);
            }
        }
        
        if(player.directionalInput.y < -0.1f)
        {
            player.ChangeState(player.crouchState);
        }
    }

    public override void FixedUpdateState()
    {


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
            player.animator.Play("standing");
            player.GroundDecelerate();
        }

        if (!player.OnTheGround())
        {
            player.ChangeState(player.airState);
        }

    }

    
}

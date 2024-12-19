using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirAttackState : PlayerState
{
    public PlayerControl.AttackType attackType;

    public override void EnterState(PlayerControl player)
    {
        base.EnterState(player);
        switch(attackType)
        {
            case PlayerControl.AttackType.neutralAir:
                player.animator.Play("airAttack");
                break;
            case PlayerControl.AttackType.upAir:
                player.animator.Play("airUpAttack");
                break;
            case PlayerControl.AttackType.downAir:
                player.animator.Play("airDownAttack");
                break;
            case PlayerControl.AttackType.backAir:
                player.animator.Play("airBackAttack");
                break;
            default:
                Debug.Log("missing air attack?");
                player.ChangeState(player.airState); 
                break;
        }
    }
    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        if (player.DirectionallyInputting())
        {
            player.AirDrift();
        }
        else
        {
            player.AirDecelerate();
        }

        if (player.OnTheGround())
        {
            switch(attackType)
            {
                case PlayerControl.AttackType.neutralAir:
                    player.landingState.lagTime = .14f;
                    break;
                default:
                    player.landingState.lagTime = .2f;
                    break;
            }
            player.ChangeState(player.landingState);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class AirState : PlayerState
{

    public override void EnterState(PlayerControl player)
    {
        base.EnterState(player);
        if(player.GetVelocity().y > 0)
        {
            player.animator.Play("airAscend");
        }
        else
        {
            player.animator.Play("airDescend");
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (!player.controlling) return;

        if (Input.GetButtonDown(player.attackButton))
        {
            if (player.DirectionallyInputting())
            {
                switch(player.GetInputDirection())
                {
                    case 1:
                        player.AirAttack(PlayerControl.AttackType.neutralAir);
                        break;
                    case -1:
                        player.AirAttack(PlayerControl.AttackType.backAir);
                        break;
                    case 2:
                        player.AirAttack(PlayerControl.AttackType.downAir);
                        break;
                    case -2:
                        player.AirAttack(PlayerControl.AttackType.downAir);
                        break;
                    case 0:
                        player.AirAttack(PlayerControl.AttackType.upAir);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                player.AirAttack(PlayerControl.AttackType.neutralAir);
            }
        }
    }

    public override void FixedUpdateState()
    {

        if(player.GetVelocity().y < -0.1)
        {
            player.animator.Play("airDescend");
        }

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
            player.touchGroundGen.Play();
            player.ChangeState(player.groundState);
        }
    }
}

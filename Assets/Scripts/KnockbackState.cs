using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackState : PlayerState
{
    public Vector2 knockbackForce;

    public override void EnterState(PlayerControl player)
    {
        base.EnterState(player);
        player.ChangeColor(Color.gray);
        player.animator.Play("stunned");
        player.TakeKnockback(knockbackForce);
        player.ToggleKnockback(true);
    }

    public override void FixedUpdateState()
    {
        player.SetVelocity(player.GetVelocity() - player.GetVelocity() * player.knockbackDecay * Time.fixedDeltaTime);

        if(player.GetVelocity().magnitude < player.knockbackEndVelocity)
        {
            if (player.OnTheGround())
            {
                player.ChangeState(player.groundState);
            }
            else
            {
                player.ChangeState(player.airState);
            }
        }

        

    }

    public override void ExitState()
    {

        player.ChangeColor(player.defaultColor);
        player.ToggleKnockback(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : PlayerState
{
    public override void UpdateState()
    {
        if(Input.GetKeyDown(KeyCode.Space) && player.controlling)
        {
            player.Jump();
        }

        if (Input.GetKeyDown(KeyCode.E) && player.controlling)
        {
            player.ChangeState(player.attackingState);
        }

    }

    public override void FixedUpdateState()
    {
        if (player.DirectionallyInputting())
        {
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

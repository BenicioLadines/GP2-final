using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundState : PlayerState
{
    public override void UpdateState()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            player.Jump();
        }

    }

    public override void FixedUpdateState()
    {
        if (player.DirectionallyInputting())
        {
            player.Walk();
        }

        if (!player.OnTheGround())
        {
            player.ChangeState(player.airState);
        }
    }
}

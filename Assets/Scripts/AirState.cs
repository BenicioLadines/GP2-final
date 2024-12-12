using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : PlayerState
{

    
    public override void FixedUpdateState()
    {
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
            player.ChangeState(player.groundState);
        }
    }
}

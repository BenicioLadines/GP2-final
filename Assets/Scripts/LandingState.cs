using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingState : PlayerState
{
    public float lagTime;

    public override void EnterState(PlayerControl player)
    {
        base.EnterState(player);
        player.HitboxDisable(0);
        //Debug.Log(lagTime);
        player.animator.Play("landingLag");
    }

    public override void UpdateState()
    {
        base.UpdateState();
        lagTime -= Time.deltaTime;

        if( lagTime < 0)
        {
            player.ChangeState(player.groundState);
        }
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
    }

}

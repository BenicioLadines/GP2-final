using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchState : PlayerState
{
    public override void EnterState(PlayerControl player)
    {
        base.EnterState(player);
        player.animator.Play("crouching");
    }

    public override void UpdateState()
    {
        if (Input.GetButtonDown(player.jumpButton))
        {
            player.Jump();
        }

        if (Input.GetButtonDown(player.attackButton))
        {
            player.Attack(PlayerControl.AttackType.downGround);
        }

        if (player.directionalInput.y >= -0.1f)
        {
            player.ChangeState(player.groundState);
        }
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        if (!player.OnTheGround())
        {
            player.ChangeState(player.airState);
        }
    }
}

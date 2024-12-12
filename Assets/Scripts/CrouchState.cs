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
        if (Input.GetKeyDown(KeyCode.E))
        {
            player.Attack(PlayerControl.AttackType.downGround);
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            player.ChangeState(player.groundState);
        }
    }
}

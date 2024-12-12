using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : PlayerState
{
    public override void EnterState(PlayerControl player)
    {
        base.EnterState(player);
        player.animator.Play("attack");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerControl;

public class AttackingState : PlayerState
{
    public PlayerControl.AttackType attackType;

    public override void EnterState(PlayerControl player)
    {
        base.EnterState(player);
        switch (attackType)
        {
            case AttackType.neutralGround:
                player.animator.Play("groundAttack");
                break;
            case AttackType.forwardGround:
                player.animator.Play("forwardKick");
                break;
            case AttackType.downGround:
                player.animator.Play("crouchAttack");
                break;
            case AttackType.upGround:
                player.animator.Play("uppercut");
                break;
            default:
                Debug.Log("missing attack?");
                player.ChangeState(player.groundState);
                break;
        }
    }
}

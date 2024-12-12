using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{

    protected PlayerControl player;

    public virtual void EnterState(PlayerControl player)
    {
        this.player = player;
    }

    public virtual void UpdateState() { }

    public virtual void FixedUpdateState() { }

    public virtual void ExitState() { }

}

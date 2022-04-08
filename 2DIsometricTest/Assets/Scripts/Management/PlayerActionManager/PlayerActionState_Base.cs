using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerActionState_Base : State_Base<PlayerActionState>
{
    public PlayerActionState_Base(PlayerActionManager pam) : base(pam) { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState_NotStarted : CombatState_Base
{
    public CombatState_NotStarted(CombatManager cm) : base(cm) { }

    public override CombatState State => CombatState.CombatState_NotStarted;

    public override void EnterState()
    {

    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {

    }
}

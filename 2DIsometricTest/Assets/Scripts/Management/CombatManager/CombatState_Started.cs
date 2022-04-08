using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState_Started : CombatState_Base
{
    public CombatState_Started(CombatManager cm) : base(cm) { }

    public override CombatState State => CombatState.CombatState_Started;

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState_Preparing : CombatState_Base
{
    private bool firstFrame = true;

    public CombatState_Preparing(CombatManager cm) : base(cm){ }

    public override CombatState State => CombatState.CombatState_Preparing;

    public override void EnterState()
    {
        firstFrame = true;
    }

    public override void UpdateState()
    {
        if (firstFrame)
        {
            firstFrame = false;
            return;
        }

        //Nothing much to do except Start
        Manager.SetState(CombatState.CombatState_Started);
    }

    public override void ExitState()
    {
        //Do nothing?
    }
}

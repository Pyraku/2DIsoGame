using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatState_Base : State_Base<CombatState>
{
    public CombatState_Base(CombatManager combatManager) : base(combatManager) { }
}

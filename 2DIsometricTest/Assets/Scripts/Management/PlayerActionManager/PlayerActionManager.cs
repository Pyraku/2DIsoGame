using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Declare PlayerActionManager states here
public enum PlayerActionState
{
    //Typical Player Actions
    PlayerActionState_Move,
    PlayerActionState_Look,
    PlayerActionState_Target,
    PlayerActionState_Skip,

    //Atypical Player Actions
    PlayerActionState_Disabled
}

public class PlayerActionManager : ManagerBase<PlayerActionState>
{
    [Inject(InjectFrom.Anywhere)]
    public InputManager m_inputManager = null;

    [Inject(InjectFrom.Anywhere)]
    public CombatManager m_combatManager = null;
}

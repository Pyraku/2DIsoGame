using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionState_Look : PlayerActionState_Base
{
    public override PlayerActionState State => PlayerActionState.PlayerActionState_Look;

    public PlayerActionState_Look(PlayerActionManager pam) : base(pam) { }

    public override void EnterState()
    {
        (Manager as PlayerActionManager).m_inputManager.PointerClick += Action;
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {
        (Manager as PlayerActionManager).m_inputManager.PointerClick -= Action;
    }

    private void Action()
    {
        //Get world space coordinates from mouse position in screen space
        Vector3 mouseWorldPosition = (Manager as PlayerActionManager).m_combatManager.m_cameraControl.GetScreenToWorldPoint();
        //Convert MouseWorldPosition into the WorldPosition struct
        WorldPosition wp = (Manager as PlayerActionManager).m_combatManager.m_world.GetNearestWorldPositionFromVector3(mouseWorldPosition);

        (Manager as PlayerActionManager).m_combatManager.m_char.m_charController.RotateCharacter(wp);
    }
}

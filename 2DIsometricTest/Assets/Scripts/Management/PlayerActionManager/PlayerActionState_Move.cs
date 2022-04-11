using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionState_Move : PlayerActionState_Base
{  
    public override PlayerActionState State => PlayerActionState.PlayerActionState_Move;

    public PlayerActionState_Move(PlayerActionManager pam) : base(pam) { }

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
        //Check with the World to see if the new worldPosition is a valid tileSpace, if not bail out of the action.
        if (!(Manager as PlayerActionManager).m_combatManager.m_world.IsValidTileSpace(wp)) return;
        //Check Character is on valid tile
        if (!(Manager as PlayerActionManager).m_combatManager.m_world.IsValidTileSpace((Manager as PlayerActionManager).m_combatManager.m_char.WorldPosition)) return;

        TileSpace start = (Manager as PlayerActionManager).m_combatManager.m_world.TileSpaces[(Manager as PlayerActionManager).m_combatManager.m_char.WorldPosition];
        TileSpace target = (Manager as PlayerActionManager).m_combatManager.m_world.TileSpaces[wp];

        //Trigger Pathfinding
        List<Vector2> path = (Manager as PlayerActionManager).m_combatManager.m_pathfinding.FindPath(start.PNode, target.PNode);

        (Manager as PlayerActionManager).m_combatManager.m_char.m_charController.StartMove(path);
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

//Declare CombatManager States here
public enum CombatState
{
    CombatState_NotStarted,
    CombatState_Preparing,
    CombatState_Started,
}

public class CombatManager : ManagerBase<CombatState>
{
    [Inject(InjectFrom.Anywhere)]
    public GameStateManager m_gameStateManager;

    [Inject(InjectFrom.Anywhere)]
    public CameraControl m_cameraControl;

    [Inject(InjectFrom.Anywhere)]
    public World m_world;

    [Inject(InjectFrom.Anywhere)]
    public PathingGrid m_pathingGrid;

    public bool InitializeWorld()
    {
        if (m_world == null || m_pathingGrid == null) return false;
        m_pathingGrid.CreateGrid();

        SetState(CombatState.CombatState_Preparing);
        m_gameStateManager.SetState(GameState.GameState_Gameplay);

        return true;
    }

}

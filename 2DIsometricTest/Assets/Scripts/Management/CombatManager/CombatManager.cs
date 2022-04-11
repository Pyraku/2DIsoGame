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
    public Pathfinding m_pathfinding;

    //Temp
    [Inject(InjectFrom.Anywhere)]
    public Character m_char;

    public bool InitializeWorld()
    {
        if (m_world == null) return false;
        m_pathfinding._Grid.CreateGrid(World.WorldWidth, World.WorldHeight);
        m_world.BuildWorld(m_pathfinding._Grid.Grid);

        //Set Camera to character
        m_cameraControl.m_focussedObject = m_char.gameObject;
        m_cameraControl.SetState(CameraState.CameraState_ObjectFocus);

        SetState(CombatState.CombatState_Preparing);
        m_gameStateManager.SetState(GameState.GameState_Gameplay);

        return true;
    }

}

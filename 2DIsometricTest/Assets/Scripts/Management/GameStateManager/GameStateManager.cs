using System;
using System.Collections.Generic;
using UnityEngine;

//Declare GameStateManager States here
public enum GameState
{
    GameState_MainMenu,
    GameState_Paused,
    GameState_LoadingCombat,
    GameState_LoadingMenu,
    GameState_Gameplay,
}

public class GameStateManager : ManagerBase<GameState>
{
    private static GameStateManager instance = null;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        instance = this;

        DontDestroyOnLoad(gameObject);
    }
}

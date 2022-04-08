using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState_Paused : GameState_Base
{
    public GameState_Paused(GameStateManager gm) : base(gm) { }

    public override GameState State => GameState.GameState_Paused;

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

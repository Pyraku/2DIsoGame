using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState_Base : State_Base<GameState>
{
    public GameState_Base(GameStateManager gsm) : base(gsm) { }
}

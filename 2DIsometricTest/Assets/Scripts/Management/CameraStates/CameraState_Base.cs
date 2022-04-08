using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CameraState_Base : State_Base<CameraState>
{
    public CameraState_Base(CameraControl cc) : base(cc) { }
}

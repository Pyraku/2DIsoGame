using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraState_ObjectFocus : CameraState_Base
{
    public CameraState_ObjectFocus(CameraControl cc) : base(cc) { }

    public override CameraState State => CameraState.CameraState_ObjectFocus;

    public override void EnterState()
    {
 
    }

    public override void UpdateState()
    {
        if ((Manager as CameraControl).m_focussedObject == null) return;
        (Manager as CameraControl).SetCameraPosition((Manager as CameraControl).m_focussedObject.transform.position);
    }

    public override void ExitState()
    {

    }
}

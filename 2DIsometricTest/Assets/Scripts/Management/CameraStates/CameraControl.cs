using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Declare CameraControl States here
public enum CameraState
{
    CameraState_CharacterFocus,
}

public class CameraControl : ManagerBase<CameraState>
{
    private Camera m_mainCam = null;

    [SerializeField] private Vector3 m_offset = Vector3.zero;

    private void Awake()
    {
        m_mainCam = GetComponent<Camera>();
        if (m_mainCam == null)
            Debug.LogError("No camera attached", gameObject);
    }
}


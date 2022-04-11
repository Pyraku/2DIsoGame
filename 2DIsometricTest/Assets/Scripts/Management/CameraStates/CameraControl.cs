using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Declare CameraControl States here
public enum CameraState
{
    CameraState_ObjectFocus,
}

public class CameraControl : ManagerBase<CameraState>
{
    private Camera m_mainCam = null;

    [SerializeField] private Vector3 m_offset = Vector3.zero;

    public GameObject m_focussedObject = null;

    private void Awake()
    {
        m_mainCam = GetComponent<Camera>();
        if (m_mainCam == null)
            Debug.LogError("No camera attached", gameObject);
    }

    public void SetCameraPosition(Vector3 p)
    {
        if (m_mainCam == null) return;

        float xPixelSize = (m_mainCam.orthographicSize * 2f) / Screen.width; // 8 / 256 = 0.03125f

        float x = p.x + m_offset.x;
        float y = p.y + m_offset.y;

       // x = Mathf.Round(x / xPixelSize) * xPixelSize;
        //y = Mathf.Round(y / xPixelSize) * xPixelSize;

        m_mainCam.transform.position = new Vector3(x, y, m_mainCam.transform.position.z);
    }

    public Vector3 GetScreenToWorldPoint()//Gets mouse position from screen and converts to world space
    {
        if (m_mainCam == null)
            return Vector3.zero;
        Vector3 mp = Input.mousePosition;

        return m_mainCam.ScreenToWorldPoint(mp);
    }
}


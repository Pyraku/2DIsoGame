using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [Inject(InjectFrom.Anywhere)]
    public GraphicRaycaster m_raycaster = null;
    [Inject(InjectFrom.Anywhere)]
    public EventSystem m_eventSystem = null;

    public delegate void PlayerInput();
    public PlayerInput PointerClick;

    private InputModule m_mouseModule = null;

    private void Start()
    {
        m_mouseModule = new MouseModule(this);
    }

    private void Update()
    {
        if (m_mouseModule != null)
            m_mouseModule.UpdateModule();
    }
}

public abstract class InputModule
{
    protected InputManager m_inputManager = null;

    public InputModule(InputManager im) { m_inputManager = im; }

    public abstract void UpdateModule();
}

public class MouseModule : InputModule
{
    private PointerEventData m_pointerEventData = null;

    public MouseModule(InputManager im) : base(im) { }

    public override void UpdateModule()
    {
        if (m_inputManager == null) return;

        if (UIRaycastCheck()) return;

        if (Input.GetMouseButtonUp(0) && m_inputManager.PointerClick != null)
            m_inputManager.PointerClick.Invoke();
    }

    protected bool UIRaycastCheck()
    {
        if (m_inputManager == null) return false;
        if (m_inputManager.m_raycaster == null && m_inputManager.m_eventSystem == null) return false;

        //If UI is blocking mouse, return

        m_pointerEventData = new PointerEventData(m_inputManager.m_eventSystem);
        m_pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        m_inputManager.m_raycaster.Raycast(m_pointerEventData, results);

        if (results.Count > 0)
            return true;

        return false;
    }
}

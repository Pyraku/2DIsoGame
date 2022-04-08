using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseControl : MonoBehaviour
{
    [Inject(InjectFrom.Anywhere)]
    public CombatManager m_combatManager;

    public Character _SelectedCharacter;
    protected const float _Height = 2.0f;
    protected const float _Width = 4.0f;
    public int _CurrentLayer;
    public Transform _Reticule;

    //UI
    [SerializeField] private GraphicRaycaster m_raycaster = null;
    [SerializeField] private EventSystem m_eventSystem = null;
    private PointerEventData m_pointerEventData = null;

    //Future plans - create an enum that links up whatever mode the mouse is in, i.e. movement, aiming, searching, etc
    public bool TargetMode
    {
        get
        {
            return _Reticule.gameObject.activeSelf;
        }
        set
        {
            _Reticule.gameObject.SetActive(value);
            Vector3 v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _Reticule.position = new Vector3(v3.x, v3.y, 0);
        }
    }

    void Update() //Get Mouse Inputs and point trigger whatever functions are associated with it
    {
        if (m_combatManager.State != CombatState.CombatState_Started) return;
        if (m_combatManager.m_gameStateManager.State != GameState.GameState_Gameplay) return;

        //If TargetMode is on
        if (TargetMode)
        {
            //Move Reticule to mouse position
            Vector3 v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _Reticule.position = new Vector3(v3.x, v3.y, 0);
        }
        
        if (Input.GetMouseButtonUp(0))
            LeftMouseClick();
        else if (Input.GetMouseButtonUp(1))
            RightMouseClick();
        else if (Input.GetMouseButtonUp(2))
            MiddleMouseClick();
    }

    protected WorldPosition GetWorldPosition()//Converts mouse click screen position into world position and then uses that to find the nearest isometric tile point 
    {
        //Gets Screen transform position for mouse click
        Vector3 v3 = Input.mousePosition;

        //Sets Z to Distance camera is away from map
        v3.z = 1.0f;// Maybe use it to for SortingLayer??

        //Converts mouse click to world position
        v3 = Camera.main.ScreenToWorldPoint(v3);
        //Gets X position rounded to nearest 0.5f
        v3.x = Mathf.Round(v3.x * _Height) / _Height;
        //Get stupid cos wave to increase y by 0.25f whenever x is a multiple of 0.5f (i.e. x = 0.0f y= 0.0f, x = 0.5f y = 0.25f, x = 1.0f y = 0.0f, etc)
        float t2 = ((Mathf.Cos(Mathf.PI * 2.0f * v3.x)) * 0.5f) + 0.5f;
        // There are two parts of this equation to be wary of, both involve t2, the first t2 MUST be inside the RoundTo1/2 function to enable the rounding area to cover the correct places
        //The second t2 is to essentially pop the y into the correct place
        v3.y = Mathf.Round((v3.y - (0.25f * t2)) * _Height) / _Height + (t2 * 0.25f) - 0.25f;
        //Return value
        return new WorldPosition(v3.x, v3.y, _CurrentLayer, 0);
    }

    protected float GetMapHeight(float x, float y)//Gets the Max or Min height of the map based on the X position, x is the X position, y determines if min or max (-1 is min, 1 is max)
    {
        //Get multipler (either 1 or -1)
        float x1 = (Mathf.Abs(x + 0.1f) / (x + 0.1f)) * _Width;
        float y1 = (Mathf.Abs(y + 0.1f) / (y + 0.1f)) * _Height;//Just for safety precaution
        return y1 - ((x * y1) / x1);
    }

    protected void MoveSelectedCharacter(WorldPosition target)//Finds whether the target position is suitable to move to and then triggers the selected character to move to that position
    {
        //First checks the target is within bounds of map, if it isn't suitable then logs the position
        if (m_combatManager.m_world.TileSpaces.ContainsKey(target))
        {
            //If target is suitable to move to, then move the character to it
            m_combatManager.m_world.PFinder.FindPath(_SelectedCharacter.WorldPosition, target);
            //StartCoroutine(_SelectedCharacter.Move(m_combatManager.m_world.PGrid.worldPath));
        }
        else
        {
            //Logs position for testing purposes
            Debug.Log("Invalid Target " + target);
        }
    }

    protected void RotateSelectedCharacter(WorldPosition target)//Points selected character in direction of mouse click
    {
        //Sends the target vector to character for calculations and execution
        //_SelectedCharacter.RotateCharacter(target);
    }

    protected void SelectTargetObject(WorldPosition target)
    {
        if (!m_combatManager.m_world.TileSpaces.ContainsKey(_SelectedCharacter.WorldPosition))
        {
            Debug.LogError("Selected Character is not currently within a tilespace");
            return;
        }
        if (!m_combatManager.m_world.TileSpaces.ContainsKey(target))
        {
            Debug.LogError("Target does not exist within tilespace");
            return;
        }
        //Character should have a reference to what tilespace they are in?<=======
        WorldPosition pos1 = _SelectedCharacter.WorldPosition;

        WorldPosition pos2 = target;

        //Use selected characters weapon to get base accuracy
        float i = _SelectedCharacter.LeftHand.GetAccuracy(pos1, pos2);

        Debug.Log("Base Accuracy is:" + i + " Real Accuracy: " + (i * GetComponent<AccuracyCalculator>().GetVision(pos1, pos2)));
    }

    #region Mouse Inputs

    protected void LeftMouseClick()
    {
        if (UIRaycastCheck()) return;
        if (TargetMode)
            SelectTargetObject(GetWorldPosition());
        else
            MoveSelectedCharacter(GetWorldPosition());
    }

    protected void RightMouseClick()
    {
        if (UIRaycastCheck()) return;
        if (!TargetMode)
            RotateSelectedCharacter(GetWorldPosition());
    }

    protected void MiddleMouseClick()
    {
        if (UIRaycastCheck()) return;
    }
    
    #endregion

    //Check if UI is blocking screen
    protected bool UIRaycastCheck()
    {
        //If UI is blocking mouse, return
        if (m_raycaster != null && m_eventSystem != null)
        {
            m_pointerEventData = new PointerEventData(m_eventSystem);
            m_pointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_raycaster.Raycast(m_pointerEventData, results);

            if (results.Count > 0)
                return true;
        }

        return false;
    }
}
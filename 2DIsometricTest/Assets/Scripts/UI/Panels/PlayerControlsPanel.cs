using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControlsPanel : BasePanel
{
    [Inject(InjectFrom.Anywhere)]
    public PlayerActionManager m_playerActionManager;

    #region Action Button refs
    //Base Action buttons
    [SerializeField] private Button m_move = null;
    [SerializeField] private Button m_look = null;
    [SerializeField] private Button m_target = null;
    [SerializeField] private Button m_skip = null;
    #endregion

    private Button m_currentSelected = null;

    private void Start()
    {
        //AssignButtonActions
        if (m_move != null)
            m_move.onClick.AddListener(MoveSelected);
        if (m_look != null)
            m_look.onClick.AddListener(LookSelected);
        if (m_target != null)
            m_target.onClick.AddListener(TargetSelected);
        if (m_skip != null)
            m_skip.onClick.AddListener(SkipSelected);
    }

    #region Action Button Methods
    public void MoveSelected()
    {
        SelectAction(m_move);
        m_playerActionManager.SetState(PlayerActionState.PlayerActionState_Move);
    }

    public void LookSelected()
    {
        SelectAction(m_look);
        m_playerActionManager.SetState(PlayerActionState.PlayerActionState_Look);
    }

    public void TargetSelected()
    {
        SelectAction(m_target);
        m_playerActionManager.SetState(PlayerActionState.PlayerActionState_Target);
    }

    public void SkipSelected()
    {
        SelectAction(m_skip);
        m_playerActionManager.SetState(PlayerActionState.PlayerActionState_Skip);
    }
    #endregion

    private void SelectAction(Button newAction)
    {
        if (m_currentSelected != null)
            m_currentSelected.interactable = true;
        m_currentSelected = newAction;
        if (m_currentSelected == null) return;
        m_currentSelected.interactable = false;
    }
}

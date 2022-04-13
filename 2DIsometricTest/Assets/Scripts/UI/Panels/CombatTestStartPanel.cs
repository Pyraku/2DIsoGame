using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatTestStartPanel : BasePanel
{
    [Inject(InjectFrom.Anywhere)]
    public CombatManager m_combatManager;

    public CyclableContainer<CombatTestSetting> m_defs = new CyclableContainer<CombatTestSetting>();

    [SerializeField] private Text m_settingName = null;

    [SerializeField] private UIGrid m_grid = null;

    protected override void OnShow()
    {
        base.OnShow();

        m_defs = new CyclableContainer<CombatTestSetting>(Resources.LoadAll<CombatTestSetting>("Scriptables/Debug"));

        UpdateUI();
    }

    private void UpdateUI()
    {
        m_settingName.text = m_defs.currentElement.Name;

        m_grid.BuildGrid(m_defs.currentElement.MapSizeX, m_defs.currentElement.MapSizeY);

        foreach (CharacterData c in m_defs.currentElement.Friends)
            m_grid.MarkCoordinates(c.m_spawn.x, c.m_spawn.y, Color.cyan);

        foreach (CharacterData e in m_defs.currentElement.Enemies)
            m_grid.MarkCoordinates(e.m_spawn.x, e.m_spawn.y, Color.red);
    }

    #region Button Actions

    public void StartCombatTest()
    {
        if(m_combatManager == null) { Debug.LogError("Combat Manager is null"); return; }
        if (!m_combatManager.InitializeWorld()) { Debug.LogError("Combat manager failed to initialize world"); return; }

        //If successful hide panel
        Hide();
    }

    public void LeftArrow()
    {
        m_defs.MovePrevious();
        UpdateUI();
    }

    public void RightArrow()
    {
        m_defs.MoveNext();
        UpdateUI();
    }

    #endregion
}
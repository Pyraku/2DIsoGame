using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTestStartPanel : BasePanel
{
    [Inject(InjectFrom.Anywhere)]
    public CombatManager m_combatManager;

    #region Button Actions

    public void StartCombatTest()
    {
        if(m_combatManager == null) { Debug.LogError("Combat Manager is null"); return; }
        if (!m_combatManager.InitializeWorld()) { Debug.LogError("Combat manager failed to initialize world"); return; }

        //If successful hide panel
        Hide();
    }

    #endregion
}

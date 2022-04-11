using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : WorldObject
{
    [SerializeField] private CharacterDef m_def = null;

    [SerializeField] protected Weapon m_leftHand = null;
    public Weapon LeftHand { get { return m_leftHand; } }

    public CharacterController m_charController { get; private set; } = null;

    public override void Initialize(World world)
    {
        base.Initialize(world);
        m_charController = GetComponent<CharacterController>();
        m_charController.Initialize(this);
    }

    protected override void SetName()//Sets GameObject name to serialized name
    {
        name = m_name;
    }

    public override void UpdateWorldPosition(WorldPosition pos)
    {
        base.UpdateWorldPosition(pos);
        if (m_leftHand != null)
            m_leftHand.InheritWorldPositon();
    }

    public void SetSprites()
    {
        if (m_def == null) { Debug.LogError("Def is null"); return; }

        m_objectSprites = new ObjectSprite[8];

        List<Sprite> s = m_def.LoadSpriteSheet();

        if (s.Count == 0) { Debug.LogError("List empty"); return; }

        for (int i = 0; i < m_objectSprites.Length; i++)
        {
            m_objectSprites[i] = new ObjectSprite((ObjectSprite.SpriteType)i, s[i]);
        }
    }

    public void LoadDef()
    {
        if (m_def == null) { Debug.LogError("Def is mising"); return; }
        SetSprites();
        m_name = m_def.Key;
        name = m_name;

        BuildSpriteDictionairy();

        UpdateWorldPosition(m_worldPosition);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : WorldObject
{
    [SerializeField] private WeaponDef m_def = null;

    //Angle range of fire based on BaseAccuracy and Range
    public float Spread
    {
        get
        {
            if (m_def == null) return 0f;
            //0--->X Range: 4 BaseAccuracy: 0.5f
            //Angle = 2f * Atan((1f/0.5f)/4)
            //Angle = 28.0724869359
            float a = 1f / m_def.BaseAccuracy;
            float b = 2f * (Mathf.Atan((a/2f)/m_def.Range));
            Debug.Log("Angle: "+b*Mathf.Rad2Deg);
            return b * Mathf.Rad2Deg;
        }
    }

    protected override void SetName()
    {
        name = m_name;
    }

    protected override void SetOrder()
    {
        //Extra 1 to always be above character
        m_sr.sortingOrder = 2 + GetOrder;
    }

    //Gets the WorldPosition of the parent
    public void InheritWorldPositon()
    {
        if(transform.parent == null)
        {
            Debug.Log("Destroying parentless Weapon");
            Destroy(gameObject);
            return;
        }
        UpdateWorldPosition(transform.parent.GetComponent<WorldObject>().WorldPosition);
    }

    //Get Accuracy of shot based on BaseAccuracy, Range and Distance from origin
    public float GetAccuracy (WorldPosition pos, WorldPosition target)
    {
        if (m_def == null) return 0f;

        Vector2 posV = m_world.TileSpaces[pos].PNode._NodePosition;
        Vector2 targetV = m_world.TileSpaces[target].PNode._NodePosition;

        InheritWorldPositon();
        //0--->X Dist: 4 Range: 4 BaseAccuracy: 0.5f
        //Get Distance from target
        float dist = Vector2.Distance(posV, targetV);//(0,0) -> (0,4) = 4f
        //Get Accuracy based upon distance
        float acc = Mathf.Clamp01((2f*m_def.BaseAccuracy) - (m_def.BaseAccuracy * (dist/m_def.Range)));

        return acc;
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

        InheritWorldPositon();
    }
}

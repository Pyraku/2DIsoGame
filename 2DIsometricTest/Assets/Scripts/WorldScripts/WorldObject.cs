using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteType = ObjectSprite.SpriteType;

public class WorldObject : MonoBehaviour
{
    public World m_world;

    protected const float m_yOffset = 0.25f;
    protected const int m_orderSpacing = 4;
    public int OrderSpacing { get { return m_orderSpacing; } }
    protected int GetOrder { get { return -m_orderSpacing * Mathf.RoundToInt(transform.position.y / m_yOffset); } }

    protected SpriteRenderer m_sr;

    [SerializeField] protected string m_name;

    [SerializeField] protected WorldPosition m_worldPosition;
    public WorldPosition WorldPosition { get { return m_worldPosition; } }

    [SerializeField] protected ObjectSprite[] m_objectSprites;//Directional Sprites from 0 - 7
    protected Dictionary<ObjectSprite.SpriteType, ObjectSprite> m_spriteIndex = new Dictionary<SpriteType, ObjectSprite>();

    [SerializeField] protected float m_radius = 0.5f;
    public float Radius { get { return m_radius; } }

    //Basic startup routine, Gets SpriteRenderer, sets layer order based on worldPos, Sets predefined name and sets sprite to default
    protected void Awake()
    {
        m_sr = GetComponent<SpriteRenderer>();
        BuildSpriteDictionairy();
    }

    public virtual void Initialize(World world)
    {
        m_world = world;
        UpdateWorldPosition(new WorldPosition(transform.position.x, transform.position.y, m_world.LayerIDs.IndexOf(m_sr.sortingLayerID), (m_worldPosition.angle == SpriteType.South) ? SpriteType.South : m_worldPosition.angle));
        SetName();
    }

    public virtual void UpdateWorldPosition(WorldPosition pos)//Updates WorldPosition to new values
    {
        if (m_sr == null)
            m_sr = GetComponent<SpriteRenderer>();
        m_worldPosition = pos;
        transform.position = m_worldPosition;
        if (m_world != null)
            m_sr.sortingLayerID = m_world.LayerIDs[pos.layer];
        SetSprite(m_worldPosition.angle);
        SetOrder();
    }

    protected virtual void SetOrder()//Sets Sorting Order based on Y position
    {
        //*-3 to reverse order from coords and to increase tile order size by 3, +1 because regular object is 2nd in order
        m_sr.sortingOrder = 1 + GetOrder;
    }

    protected virtual void SetName()//Sets Name based on name defined in _Name
    {
        name = m_name;
        m_world.RegisterNewObject(this);
    }

    public void SetSprite(SpriteType type)
    {
        if (!m_spriteIndex.ContainsKey(type))
        {
            if (m_objectSprites.Length > (int)type)
            {
                m_sr.sprite = m_objectSprites[(int)type]._Sprite;
                return;
            }
            else
                return;
        }
        m_sr.sprite = m_spriteIndex[type]._Sprite;
    }

    protected void BuildSpriteDictionairy()
    {
        m_spriteIndex = new Dictionary<SpriteType, ObjectSprite>();
        foreach (ObjectSprite o in m_objectSprites)
        {
            if (m_spriteIndex.ContainsKey(o.m_spriteType))
            {
                Debug.LogError("SpriteType already exists in index " + name);
                continue;
            }
            m_spriteIndex.Add(o.m_spriteType, o);
        }
    }
}

[System.Serializable]
public struct ObjectSprite//Basic container for sprites, includes naming capability so you don't have to figure out what the numbers mean
{
    public enum SpriteType
    {
        South = 0,
        SouthEast = 1,
        East = 2,
        NorthEast = 3,
        North = 4,
        NorthWest = 5,
        West = 6,
        SouthWest = 7,
        Default = -1
    }

    public string _Name;

    public SpriteType m_spriteType;

    public Sprite _Sprite;

    public ObjectSprite(SpriteType type, Sprite sprite)
    {
        m_spriteType = type;
        _Sprite = sprite;

        _Name = type.ToString();
    }
}

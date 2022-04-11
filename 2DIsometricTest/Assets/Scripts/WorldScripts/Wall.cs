using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteType = ObjectSprite.SpriteType;

public class Wall : WorldObject {

    protected int m_order;
    public int Order { get { return m_order; } set { m_order = value; SetOrder(); } }

    protected bool m_isPassable = false;
    public bool IsPassable { get { return m_isPassable; } }

    public override void Initialize(World world)
    {
        base.Initialize(world);
    }
 
    public override void UpdateWorldPosition(WorldPosition pos)
    {
        base.UpdateWorldPosition(pos);
        SetName();
    }

    protected override void SetOrder ()
    {
        //*-3 to reverse order from coords and to increase tile order size by 3, +0 because North Wall object is 1st in order
        if (m_sr == null)
            m_sr = GetComponent<SpriteRenderer>();
        m_sr.sortingOrder = (2 * m_order) + GetOrder;
    }

    protected override void SetName()
    {
        name = m_name + "(x:" + m_worldPosition.x + ",y:" + m_worldPosition.y + ")";
    }

    public virtual void TogglePassable(bool value)
    {
        m_isPassable = value;
    }

    public bool CheckPassability(SpriteType dir)
    {
        if (IsPassable) return true;
        if(m_worldPosition.angle == SpriteType.South || m_worldPosition.angle == SpriteType.East || m_worldPosition.angle == SpriteType.North || m_worldPosition.angle == SpriteType.West)
            return false;
        //Get int of angle
        int ang = (int)dir;

        //Check left of angle
        int left = (int)m_worldPosition.angle + 1;
        if (left > 7)
            left -= 8;
        if (left == ang)
            return false;
        
        //Check angle
        if ((int)m_worldPosition.angle == ang)
            return false;
        
        //Check right of angle
        int right = (int)m_worldPosition.angle - 1;
        if (right < 0)
            right += 8;
        if (right == ang)
            return false;

        //Else return true TRUE IS PASSABLE
        return true;
    }

    public Vector4 GetBounds()
    {
        Vector4 temp = new Vector4();
        switch (m_worldPosition.angle)
        {
            case SpriteType.South:
            case SpriteType.North:
                temp = new Vector4(m_worldPosition.x - 0.5f, m_worldPosition.y, m_worldPosition.x + 0.5f, m_worldPosition.y);
                break;
            case SpriteType.East:
            case SpriteType.West:
                temp = new Vector4(m_worldPosition.x, m_worldPosition.y-0.25f, m_worldPosition.x, m_worldPosition.y + 0.25f);
                break;
            case SpriteType.SouthEast:
                temp = new Vector4(m_worldPosition.x, m_worldPosition.y-0.25f, m_worldPosition.x + 0.5f, m_worldPosition.y);
                break;
            case SpriteType.NorthEast:
                temp = new Vector4(m_worldPosition.x, m_worldPosition.y + 0.25f, m_worldPosition.x + 0.5f, m_worldPosition.y);
                break;
            case SpriteType.NorthWest:
                temp = new Vector4(m_worldPosition.x, m_worldPosition.y + 0.25f, m_worldPosition.x - 0.5f, m_worldPosition.y);
                break;
            case SpriteType.SouthWest:
                temp = new Vector4(m_worldPosition.x, m_worldPosition.y-0.25f, m_worldPosition.x-0.5f, m_worldPosition.y);
                break;
        }
        return temp;
    }

    public List<Vector2> GetNBounds()
    {
        List<Vector2> temp = new List<Vector2>();
        switch (m_worldPosition.angle)
        {
            case SpriteType.South:
            case SpriteType.North:
                temp.Add(new Vector2(-0.5f, 0.5f));
                temp.Add(new Vector2(0.5f, -0.5f));
                break;
            case SpriteType.East:
            case SpriteType.West:
                temp.Add(new Vector2(0.5f, 0.5f));
                temp.Add(new Vector2(-0.5f, -0.5f));
                break;
            case SpriteType.SouthEast:
                temp.Add(new Vector2(-0.5f, -0.5f));
                temp.Add(new Vector2(0.5f, -0.5f));
                break;
            case SpriteType.NorthEast:
                temp.Add(new Vector2(0.5f, -0.5f));
                temp.Add(new Vector2(0.5f, 0.5f));
                break;
            case SpriteType.NorthWest:
                temp.Add(new Vector2(0.5f, 0.5f));
                temp.Add(new Vector2(-0.5f, 0.5f));
                break;
            case SpriteType.SouthWest:
                temp.Add(new Vector2(-0.5f, -0.5f));
                temp.Add(new Vector2(-0.5f, 0.5f));
                break;
        }
        return temp;
    }
}

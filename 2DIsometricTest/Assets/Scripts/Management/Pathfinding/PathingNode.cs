using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathingNode
{
    public bool _Walkable
    {
        get
        {
            if (m_tileSpace == null) return false;
            return !m_tileSpace.IsBlockedByObject();
        }
    }

    public Vector2 _NodePosition;
    public int _GridX, _GridY;

    public int _GCost, _HCost;
    public PathingNode _Parent;

    public int _FCost => _GCost + _HCost;

    private TileSpace m_tileSpace = null;
    public TileSpace TSpace { get { return m_tileSpace; } }

    public PathingNode(Vector2 nodePosition, int x, int y)
    {
        _NodePosition = nodePosition;
        _GridX = x;
        _GridY = y;
    }

    public void AssignTileSpace(TileSpace ts)
    {
        m_tileSpace = ts;
        ts.AddNewPathingNode(this);
    }
}

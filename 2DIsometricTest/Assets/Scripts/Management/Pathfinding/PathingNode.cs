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

    private TileSpace m_tileSpace = null;
    public TileSpace TSpace { get { return m_tileSpace; } }

    //Sets up new values for a new node
    public PathingNode(Vector2 nodePosition, WorldPosition worldPosition, int gridX, int gridY, TileSpace tileSpace)
    {
        _NodePosition = nodePosition;
        _GridX = gridX;
        _GridY = gridY;
        m_tileSpace = tileSpace;
    }

    //Gets the F cost of the node
    public int _FCost
    {
        get
        {
            return _GCost + _HCost;
        }
    }
}

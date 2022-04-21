using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteType = ObjectSprite.SpriteType;

public class PathingGrid : MonoBehaviour
{
    public Vector2 _GridWorldSize;
    public float _NodeRadius;

    private PathingNode[,] m_grid;
    public PathingNode[,] Grid { get { return m_grid; } }

    private float NodeDiameter => _NodeRadius * 2f;
    int _GridSizeX, _GridSizeY;

    [SerializeField] private bool m_diagonalOn = true;

    public void CreateGrid(int w, int h)
    {
        _GridSizeX = w;
        _GridSizeY = h;
        m_grid = new PathingNode[w, h];
        for (int x = 0; x < _GridSizeX; x++)
        {
            for (int y = 0; y < _GridSizeY; y++)
            {
                Vector2 _WorldPoint = Vector2.right * (x * NodeDiameter + _NodeRadius) + Vector2.up * (y * NodeDiameter + _NodeRadius);
                PathingNode newNode = new PathingNode(_WorldPoint, x, y);
                m_grid[x, y] = newNode;
            }
        }
    }

    //Checks for walls using position and angle of wall, will check angle, one below and one above
    public bool CheckWalls(PathingNode node, SpriteType angle)
    {
        if (node == null || node.TSpace == null)
        {
            Debug.LogError("Node isn't setup");
            return false;
        }
        return node.TSpace.IsBlockedByWall(angle);
    }

    //Gets a list of nodes surrounding target node
    public List<PathingNode> GetNeighbours(PathingNode node)
    {
        List<PathingNode> neighbors = new List<PathingNode>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                //If diagonal is off
                if (!m_diagonalOn)
                    if (!Equals(x, 0) && !Equals(y, 0))
                        continue;

                int checkx = node._GridX + x;
                int checky = node._GridY + y;

                if (checkx >= 0 && checkx < _GridSizeX && checky >= 0 && checky < _GridSizeY)
                {
                    neighbors.Add(m_grid[checkx, checky]);
                }
            }
        }
        return neighbors;
    }

    //Debug
    //public List<Vector2> worldPath;
    //public List<PathingNode> path;
    //public List<PathingNode> angles1;
    //public List<PathingNode> angles2;
    //void OnDrawGizmos()
    //{
    //    if (m_grid != null)
    //    {
    //        foreach (PathingNode node in m_grid)
    //        {
    //            Gizmos.color = (node._Walkable) ? Color.white : Color.red;
    //            if (path != null)
    //            {
    //                if (path.Contains(node))
    //                {
    //                    Gizmos.color = Color.black;
    //                }
    //                if (node._NodePosition == ConvertToNodePosition(_Player.WorldPosition))
    //                {
    //                    Gizmos.color = Color.cyan;
    //                }
    //            }
    //            Gizmos.DrawCube(node._NodePosition, Vector2.one * (_NodeDiameter - 0.1f));
    //            if (angles1.Contains(node))
    //            {
    //                Gizmos.color = Color.yellow;
    //                Gizmos.DrawCube(node._NodePosition, Vector2.one * (_NodeDiameter - 0.2f));
    //            }
    //            if (angles2.Contains(node))
    //            {
    //                Gizmos.color = Color.magenta;
    //                Gizmos.DrawCube(node._NodePosition, Vector2.one * (_NodeDiameter - 0.2f));
    //            }
    //        }
    //    }
    //}
}

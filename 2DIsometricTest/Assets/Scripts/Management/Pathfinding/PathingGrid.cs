using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteType = ObjectSprite.SpriteType;

public class PathingGrid : MonoBehaviour
{
    [Inject(InjectFrom.Anywhere)]
    public World m_world;

    public Vector2 _GridWorldSize;
    public float _NodeRadius;

    private PathingNode[,] m_grid;
    public PathingNode[,] Grid { get { return m_grid; } }

    public Character _Player;
    float _NodeDiameter;
    int _GridSizeX, _GridSizeY;

    [SerializeField] private bool m_diagonalOn = true;

    public Vector3 _Conversion;//x = x, y = y, z = angle

    void Start()
    {
        //Set all the important stuff
        _NodeDiameter = _NodeRadius * 2;
        _GridSizeX = Mathf.RoundToInt(_GridWorldSize.x / _NodeDiameter);
        _GridSizeY = Mathf.RoundToInt(_GridWorldSize.y / _NodeDiameter);
    }

    //Creates Grid from nodes using _GridSizeX and _GridSizeY
    public void CreateGrid()
    {
        m_grid = new PathingNode[_GridSizeX, _GridSizeY];
        Vector2 _WorldBottomLeft = new Vector2(0, 0);//new Vector2(transform.position.x,transform.position.y) - Vector2.right * _GridWorldSize.x / 2 - Vector2.up * _GridWorldSize.y / 2;

        for (int x = 0; x < _GridSizeX; x++)
        {
            for (int y = 0; y < _GridSizeY; y++)
            {
                Vector2 _WorldPoint = _WorldBottomLeft + Vector2.right * (x * _NodeDiameter + _NodeRadius) + Vector2.up * (y * _NodeDiameter + _NodeRadius);
                WorldPosition _WorldPos = ConvertToWorldPosition(_WorldPoint);
                if (m_world.TileSpaces.ContainsKey(_WorldPos))
                {
                    TileSpace ts = m_world.TileSpaces[_WorldPos];
                    PathingNode newNode = new PathingNode(_WorldPoint, _WorldPos, x, y, ts);
                    if(m_world.TileSpaces[_WorldPos].AddNewPathingNode(newNode))
                        m_grid[x, y] = newNode;
                }
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

    //Use this to convert node coordinates into level WorldPosition
    public WorldPosition ConvertToWorldPosition(Vector2 value)
    {
        float sin = Mathf.Sin(_Conversion.z * Mathf.Deg2Rad);
        float cos = Mathf.Cos(_Conversion.z * Mathf.Deg2Rad);

        float x = (value.x * cos - value.y * sin) * Mathf.Sqrt(_Conversion.x);
        float y = (value.x * sin + value.y * cos) * Mathf.Sqrt(_Conversion.x) * _Conversion.x - _Conversion.y;

        x = Mathf.Round(x / _Conversion.x) * _Conversion.x;
        y = Mathf.Round(y / _Conversion.y) * _Conversion.y;

        return new WorldPosition(x, y, m_world.CurrentLayer, 0);
    }

    //Use this to convert level WorldPosition into node coordinates
    public Vector2 ConvertToNodePosition(WorldPosition value)
    {
        float sin = Mathf.Sin(-_Conversion.z * Mathf.Deg2Rad);
        float cos = Mathf.Cos(-_Conversion.z * Mathf.Deg2Rad);

        float x = value.x / Mathf.Sqrt(_Conversion.x);
        float y = ((value.y + _Conversion.y) / _Conversion.x) / Mathf.Sqrt(_Conversion.x);

        float nx = x * cos - y * sin;
        float ny = x * sin + y * cos;

        return new Vector2(nx, ny);
    }

    public List<Vector2> worldPath;
    public List<PathingNode> path;
    public List<PathingNode> angles1;
    public List<PathingNode> angles2;
    void OnDrawGizmos()
    {
        if (m_grid != null)
        {
            foreach (PathingNode node in m_grid)
            {
                Gizmos.color = (node._Walkable) ? Color.white : Color.red;
                if (path != null)
                {
                    if (path.Contains(node))
                    {
                        Gizmos.color = Color.black;
                    }
                    if (node._NodePosition == ConvertToNodePosition(_Player.WorldPosition))
                    {
                        Gizmos.color = Color.cyan;
                    }
                }
                Gizmos.DrawCube(node._NodePosition, Vector2.one * (_NodeDiameter - 0.1f));
                if (angles1.Contains(node))
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawCube(node._NodePosition, Vector2.one * (_NodeDiameter - 0.2f));
                }
                if (angles2.Contains(node))
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawCube(node._NodePosition, Vector2.one * (_NodeDiameter - 0.2f));
                }
            }
        }
    }
}

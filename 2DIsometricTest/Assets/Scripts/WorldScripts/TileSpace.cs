using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteType = ObjectSprite.SpriteType;

public class TileSpace
{
    protected Floor m_floor = null;
    public Floor Floor { get { return m_floor; } }

    protected Dictionary<SpriteType,Wall> m_walls = new Dictionary<SpriteType, Wall>();
    public Dictionary<SpriteType,Wall> Walls { get { return m_walls; } }

    protected List<WorldObject> m_objects = new List<WorldObject>();
    public List<WorldObject> Objects { get { return m_objects; } }

    protected Vector2 m_position;
    public Vector2 Position { get { return m_position; } }

    protected PathingNode m_pathingNode = null;
    public PathingNode PNode { get { return m_pathingNode; } }

    public TileSpace(WorldObject obj)
    {
        m_objects.Add(obj);
        m_position = obj.WorldPosition;
    }

    public TileSpace(Wall wall)
    {
        m_walls.Add(wall.WorldPosition.angle,wall);
        m_position = wall.WorldPosition;
    }

    public TileSpace(Floor newFloor)
    {
        m_floor = newFloor;
        m_position = newFloor.WorldPosition;
    }

    public bool AddNewFloor(Floor floor)
    {
        if(m_floor != null)
        {
            Debug.LogError("Floor already exists");
            return false;
        }
        m_floor = floor;
        return true;
    }

    public bool AddNewWall (Wall wall)
    {
        if (m_walls.ContainsKey(wall.WorldPosition.angle))
        {
            Debug.LogError("Wall at this angle already exists on tile space " + wall.WorldPosition.angle + " " + wall.name);
            return false;
        }
        m_walls.Add(wall.WorldPosition.angle,wall);
        return true;
    }

    public bool AddNewObject (WorldObject newObject)
    {
        if (m_objects.Contains(newObject))
        {
            Debug.LogError("Object already exists in list " + newObject.name);
            return false;
        }
        m_objects.Add(newObject);
        return true;
    }

    public bool AddNewPathingNode(PathingNode newNode)
    {
        if (m_pathingNode != null)
        {
            Debug.LogError("PathingNode already exists in TileSpace");
            return false;
        }
        m_pathingNode = newNode;
        return true;
    }

    public bool IsBlockedByObject()
    {
        if (m_objects.Count == 0) return false;
        //In future, will need to check if object is big enough to be considered 'blocked'
        //for now if an object exists on this space its blocked
        return true;
    }

    public bool IsBlockedByWall(SpriteType angle)
    {
        int ang = (int)angle;
        for (int a = 0; a < 8; a++)
        {
            if (!m_walls.ContainsKey((SpriteType)a))
                continue;
            if (!m_walls[(SpriteType)a].CheckPassability((ObjectSprite.SpriteType)ang))
                return true;//TRUE MEANS THERE IS A WALL IN WAY
        }
        return false;
    }

    public List<Vector4> GetWallBounds()
    {
        List<Vector4> temp = new List<Vector4>();
        foreach (KeyValuePair<SpriteType,Wall> w in m_walls)
        {
            temp.Add(w.Value.GetBounds());
        }
        return temp;
    }

    public List<Vector2> GetNPointsOfWalls()
    {
        List<Vector2> temp = new List<Vector2>();
        foreach (KeyValuePair<SpriteType, Wall> w in m_walls)
            temp.AddRange(w.Value.GetNBounds());
        return temp;
    }
}

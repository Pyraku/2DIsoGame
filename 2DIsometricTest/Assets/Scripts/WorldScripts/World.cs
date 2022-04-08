using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteType = ObjectSprite.SpriteType;

public class World : MonoBehaviour
{
    public Dictionary<Vector2, TileSpace> TileSpaces { get; } = new Dictionary<Vector2, TileSpace>();
    public List<int> LayerIDs { get; } = new List<int>();
    public int CurrentLayer { get; } = 1;
    public PathingGrid PGrid { get; private set; } = null;
    public Pathfinding PFinder { get; private set; } = null;

    private void Awake()
    {
        foreach (SortingLayer id in SortingLayer.layers)
        {
            LayerIDs.Add(id.id);
        }
        PGrid = GetComponent<PathingGrid>();
        PFinder = GetComponent<Pathfinding>();
    }

    #region TileSpace Functions
    public bool RegisterNewFloor(Floor floor)
    {
        if (TileSpaces.ContainsKey(floor.WorldPosition))
            return TileSpaces[floor.WorldPosition].AddNewFloor(floor);
        TileSpace temp = new TileSpace(floor);
        TileSpaces.Add(temp.Position, temp);
        return true;
    }

    public bool RegisterNewWall(Wall wall)
    {
        if (TileSpaces.ContainsKey(wall.WorldPosition))
            return TileSpaces[wall.WorldPosition].AddNewWall(wall);
        TileSpace temp = new TileSpace(wall);
        TileSpaces.Add(temp.Position, temp);
        return true;
    }

    public bool RegisterNewObject(WorldObject obj)
    {
        if (TileSpaces.ContainsKey(obj.WorldPosition))
            return TileSpaces[obj.WorldPosition].AddNewObject(obj);
        TileSpace temp = new TileSpace(obj);
        TileSpaces.Add(temp.Position,temp);
        return true;
    }
    #endregion
}

[System.Serializable]
public struct WorldPosition//Essentially a way of storing position (x,y) and sorting layer ID (layer), without using a Vector3 (plus its serializable)
{
    public float x;
    public float y;
    public int layer;
    public SpriteType angle;

    public WorldPosition(float vx, float vy, int l, SpriteType ang)
    {
        x = vx;
        y = vy;
        layer = l;
        angle = ang;
    }

    public WorldPosition(float vx, float vy, int l, int ang)
    {
        x = vx;
        y = vy;
        layer = l;
        angle = (SpriteType)ang;
    }

    public WorldPosition(Vector2 v)
    {
        x = v.x;
        y = v.y;
        layer = 0;
        angle = 0;
    }

    public WorldPosition(WorldPosition wp)
    {
        x = wp.x;
        y = wp.y;
        layer = wp.layer;
        angle = wp.angle;
    }

    public void SetAngle(int ang)
    {
        angle = (SpriteType)ang;
    }

    public override string ToString()
    {
        return "(X: " + x + ", Y: " + y + ", LayerID: " + layer + ", Angle: " + angle +")";
    }

    public static WorldPosition operator + (WorldPosition a, Vector2 b)
    {
        return new WorldPosition(a.x + b.x, a.y + b.y, a.layer, a.angle);
    }

    public static implicit operator Vector3 (WorldPosition w)
    {
        return new Vector3(w.x,w.y,0);
    }

    public static implicit operator Vector2 (WorldPosition w)
    {
        return new Vector2(w.x,w.y);
    }

    public static implicit operator WorldPosition(Vector2 v)
    {
        return new WorldPosition(v);
    }
}

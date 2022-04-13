using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteType = ObjectSprite.SpriteType;

public class World : MonoBehaviour
{
    [Inject(InjectFrom.Anywhere)]
    public Floor[] m_floors;

    [Inject(InjectFrom.Anywhere)]
    public WorldObject[] m_worldObjects;

    [Inject(InjectFrom.Anywhere)]
    public WorldAssetManager m_worldAssetManager;

    public Dictionary<Vector2, TileSpace> TileSpaces { get; } = new Dictionary<Vector2, TileSpace>();
    public List<int> LayerIDs { get; } = new List<int>();
    public int CurrentLayer { get; } = 1;

    //Tile Size
    public static readonly float TileHeight = 2.0f, TileWidth = 4.0f;

    //Test World Width/Height
    public static int WorldWidth, WorldHeight; // 50 x 50 

    private void Awake()
    {
        foreach (SortingLayer id in SortingLayer.layers)
        {
            LayerIDs.Add(id.id);
        }
    }

    public void SetupWorld(CombatTestSetting setting)
    {
        if(setting == null) { Debug.LogError("setting is null"); return; }
        WorldWidth = setting.MapSizeX;
        WorldHeight = setting.MapSizeY;

    }

    public void BuildWorld(PathingNode[,] nodes)
    {
        int requiredFloors = WorldWidth * WorldHeight;
        if(m_floors.Length < requiredFloors)
        {
            Debug.Log("Not enough floors, making more");
            //Make more
        }

        for(int x = 0; x < WorldWidth; x++)
            for(int y = 0; y < WorldHeight; y++)
            {
                float a = (x / TileHeight) - (y / TileHeight);
                float b = (x / TileWidth) + (y / TileWidth);
                m_floors[(y * WorldWidth) + x].UpdateWorldPosition(new WorldPosition(a, b, 1, 0));
                RegisterNewFloor(m_floors[(y * WorldWidth) + x]);
                if (TileSpaces.ContainsKey(new Vector2(a, b)))
                    nodes[x, y].AssignTileSpace(TileSpaces[new Vector2(a, b)]);
            }

        foreach (WorldObject w in m_worldObjects)
            w.Initialize(this);
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
        TileSpaces.Add(temp.Position, temp);
        return true;
    }
    #endregion

    public WorldPosition GetNearestWorldPositionFromVector3(Vector3 target)
    {
        target.x = Mathf.Round(target.x * TileHeight) / TileHeight;
        //Get stupid cos wave to increase y by 0.25f whenever x is a multiple of 0.5f (i.e. x = 0.0f y= 0.0f, x = 0.5f y = 0.25f, x = 1.0f y = 0.0f, etc)
        float t2 = ((Mathf.Cos(Mathf.PI * 2.0f * target.x)) * 0.5f) + 0.5f;
        // There are two parts of this equation to be wary of, both involve t2, the first t2 MUST be inside the RoundTo1/2 function to enable the rounding area to cover the correct places
        //The second t2 is to essentially pop the y into the correct place
        target.y = Mathf.Round((target.y - (0.25f * t2)) * TileHeight) / TileHeight + (t2 * 0.25f) - 0.25f;
        //Return value
        return new WorldPosition(target.x, target.y, CurrentLayer, 0);
    }

    public bool IsValidTileSpace(WorldPosition wp)
    {
        return TileSpaces.ContainsKey(wp);
    }
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
        return "(X: " + x + ", Y: " + y + ", LayerID: " + layer + ", Angle: " + angle + ")";
    }

    public static WorldPosition operator +(WorldPosition a, Vector2 b)
    {
        return new WorldPosition(a.x + b.x, a.y + b.y, a.layer, a.angle);
    }

    public static implicit operator Vector3(WorldPosition w)
    {
        return new Vector3(w.x, w.y, 0);
    }

    public static implicit operator Vector2(WorldPosition w)
    {
        return new Vector2(w.x, w.y);
    }

    public static implicit operator WorldPosition(Vector2 v)
    {
        return new WorldPosition(v);
    }
}

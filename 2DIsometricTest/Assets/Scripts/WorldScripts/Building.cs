using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [Inject(InjectFrom.Anywhere)]
    public World m_world;

    [SerializeField] protected List<WorldPosition> m_wallPos = new List<WorldPosition>();
    protected List<Wall> m_walls = new List<Wall>();
    protected Vector2 m_origin;
    [SerializeField] protected string m_name;

    [SerializeField] protected Color m_colorOverride = Color.white;

	void Start ()
    {
        transform.position = m_origin;
        name = m_name;
        BuildRoom();
	}

    //Builds the designed room using its children, walls and windows are currently the only objects that are recognised to block the pathfinding
    protected void BuildRoom()
    {
        for (int i = 0; i < m_wallPos.Count; i++)
        {
            m_walls.Add(transform.GetChild(i).GetComponent<Wall>());
            m_walls[i].Order = Mathf.Abs(Mathf.RoundToInt((1.5f * Mathf.Sin(((Mathf.PI * (int)m_wallPos[i].angle) / 3.0f) + 0.5f)) + 0.5f));
            m_walls[i].UpdateWorldPosition(m_wallPos[i] + m_origin);
            if (m_walls[i].GetComponent<Door>())
            {
                m_walls[i].TogglePassable(true);
            }
            m_world.RegisterNewWall(m_walls[i]);
        }
    }

    public void DebugBuildRoom()
    {
        if (!Equals(transform.childCount, m_wallPos.Count))
        {
            if(transform.childCount > m_wallPos.Count)
            {
                Debug.LogError("You appear to have more children than wall positions");
                return;
            }
            Debug.LogError("You appear to have less children than wall positions");
            return;
        }

        Wall temp = null;
        for (int i = 0; i < m_wallPos.Count; i++)
        {
            temp = transform.GetChild(i).GetComponent<Wall>();
            if(temp == null)
            {
                Debug.LogError("Child number "+ i + " does not contain the right script");
                return;
            }
            temp.Order = Mathf.Abs(Mathf.RoundToInt((1.5f * Mathf.Sin(((Mathf.PI * (int)m_wallPos[i].angle) / 3.0f) + 0.5f)) + 0.5f));
            temp.UpdateWorldPosition(m_wallPos[i] + m_origin);
            temp.GetComponent<SpriteRenderer>().color = m_colorOverride;
        }
    }
}

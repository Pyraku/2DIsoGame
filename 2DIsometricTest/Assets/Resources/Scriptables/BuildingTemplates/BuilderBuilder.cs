using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderBuilder : MonoBehaviour
{
    [Inject(InjectFrom.Anywhere)]
    public WorldAssetManager m_wam;
    
    [SerializeField] private BuildingTemplate m_template = null;

    private Vector2Int m_gridSize = Vector2Int.zero;

    private int[,] m_grid = new int[0,0];

    private Vector2Int GetGridSize()
    {
        if (m_template == null) return Vector2Int.zero;

        int xSize = 0;
        int ySize = 0;

        foreach(Positioning p in m_template.WallSculpt)
        {
            if (p.m_x + p.m_w > xSize)
                xSize = p.m_x + p.m_w;
            if (p.m_y + p.m_h > ySize)
                ySize = p.m_y + p.m_h;
        }

        return new Vector2Int(xSize, ySize);
    }

    public void BuildFromTemplate()
    {
        m_gridSize = GetGridSize();
        m_grid = new int[m_gridSize.x, m_gridSize.y];
        foreach (Positioning p in m_template.WallSculpt)
        {
            SetGroupToGrid(p.m_materialType, new Vector2Int(p.m_x, p.m_y), new Vector2Int(p.m_w, p.m_h));
            switch (p.m_materialType)
            {
                case MaterialType.Wall:
                case MaterialType.Broken:
                    
                    break;
                case MaterialType.Empty:
                    break;
            }
        }
    }

    private void SetGroupToGrid(MaterialType type, Vector2Int pos, Vector2Int scale)
    {
        for(int y = pos.y; y > pos.y + scale.y; y++)
        {
            for(int x = pos.x; x > pos.x + scale.x; x++)
            {
                m_grid[pos.x + x, pos.y + y] = (int)type;
            }
        }
    }
}

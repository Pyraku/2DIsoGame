using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class UIGrid : MonoBehaviour
{
    [SerializeField] private GameObject m_tile = null;
    [SerializeField] private Image m_backing = null;
    [SerializeField] private float m_tilesize = 16f;

    [SerializeField] private Vector2Int m_gridSize;
    public Vector2Int GridSize => m_gridSize;

    public Image[] m_imageGrid;

    public void BuildGrid(int x, int y)
    {
        m_gridSize = new Vector2Int(x, y);

        int count = x * y;

        m_backing.rectTransform.sizeDelta = new Vector2((x + 1) * m_tilesize, (y + 1) * m_tilesize);

        List<Image> childs = new List<Image>();
        childs.AddRange(GetComponentsInChildren<Image>());
        childs.Remove(m_backing);

        childs.ForEach(x => x.enabled = false);

        if (childs.Count < count)
        {
            while (childs.Count < count)
            {
                Image newImage = Instantiate(m_tile, transform).GetComponent<Image>();
                childs.Add(newImage);
            }
        }

        m_imageGrid = childs.ToArray();

        for (int a = 0; a < y; a++)
            for (int b = 0; b < x; b++)
            {
                childs[(a * x) + b].enabled = true;
                childs[(a * x) + b].rectTransform.localPosition = new Vector3((b * m_tilesize) - (m_backing.rectTransform.sizeDelta.x / 2f) + (m_tilesize / 2f),
                    (a * m_tilesize) - (m_backing.rectTransform.sizeDelta.y / 2f) + (m_tilesize / 2f), 0f);
                childs[(a * x) + b].GetComponentInChildren<Text>().enabled = false;
                childs[(a * x) + b].color = Color.white;
            }
    }

    public void ClearGrid()
    {
        foreach (Image i in m_imageGrid)
            DestroyImmediate(i.gameObject);

        m_imageGrid = new Image[0];
    }

    public void MarkCoordinates(int x, int y, Color color)
    {
        if (x >= m_gridSize.x || y >= m_gridSize.y || x < 0 || y < 0) { Debug.LogError("Out of range"); return; }
        int i = (y * m_gridSize.x) + x;
        if (m_imageGrid[i] != null)
            m_imageGrid[i].color = color;
    }
}

[CustomEditor(typeof(UIGrid))]
public class UIGridEditor : Editor
{
    UIGrid grid;

    private void OnEnable()
    {
        grid = (UIGrid)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Refresh Grid", new GUILayoutOption[0]))
            grid.BuildGrid(grid.GridSize.x, grid.GridSize.y);

        if (GUILayout.Button("Clear Grid", new GUILayoutOption[0]))
            grid.ClearGrid();
    }
}
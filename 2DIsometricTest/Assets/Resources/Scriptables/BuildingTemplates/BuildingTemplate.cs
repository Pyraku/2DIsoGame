using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MaterialType
{
    Empty,
    Wall,
    Broken,
}

public class BuildingTemplate : ScriptableObject
{
    [SerializeField] private List<Positioning> m_wallSculpt = new List<Positioning>();
    public List<Positioning> WallSculpt => m_wallSculpt;
}

[System.Serializable]
public struct Positioning
{
    public string m_name;

    public int m_x, m_y, m_w, m_h;

    //Material type
    public MaterialType m_materialType;

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Combat Setting", menuName = "Debug/CombatTestSetting")]
public class CombatTestSetting : ScriptableObject
{
    [SerializeField] private string m_name = "Default";
    public string Name => m_name;

    [SerializeField] private int m_mapSizeX = 9, m_mapSizeY = 9;
    public int MapSizeX => m_mapSizeX;
    public int MapSizeY => m_mapSizeY;

    [SerializeField] private List<CharacterData> m_friends = new List<CharacterData>();
    public List<CharacterData> Friends => m_friends;

    [SerializeField] private List<CharacterData> m_enemies = new List<CharacterData>();
    public List<CharacterData> Enemies => m_enemies;
}

[System.Serializable]
public struct CharacterData
{
    public string m_name;
    public CharacterDef m_def;
    public WeaponDef m_wep;
    public Vector2Int m_spawn;
}

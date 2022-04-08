using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System.Linq;

[CreateAssetMenu(fileName = "WeaponDef", menuName = "ObjectScripts/WeaponDef", order =1)]
public class WeaponDef : ScriptableObject
{
    [SerializeField] private string m_key = "";
    public string Key => m_key;

    [SerializeField] private float m_range = 1;//Max Distance away from Origin that weapon can hit without accuracy and damage penalties
    public float Range => m_range;

    [SerializeField] private int m_minBaseDamage = 1;//Minimum damage weapon can do before buffs and debuffs apply
    public int MinBaseDamage => m_minBaseDamage;

    [SerializeField] private int m_maxBaseDamage = 1;//Maximum damage weapon can do before buffs and debuffs apply
    public int MaxBaseDamage => m_maxBaseDamage;

    [SerializeField] private float m_baseAccuracy = 1;//Accuracy of weapon at Max Range before distance, the users handling and terrain modifiers are applied
    public float BaseAccuracy => m_baseAccuracy;

    [SerializeField] private int m_ammoCount = 1;//Amount of times the weapon can be used before it needs to be reloaded
    public int AmmoCount => m_ammoCount;

    [SerializeField] private int m_fireRate = 1;//How many times the weapon is triggered in 1 turn
    public int FireRate => m_fireRate;

    [SerializeField] private SpriteAtlas m_atlas = null;
    [SerializeField] private int m_atlasSet = 0; 

    //Loads the correct sprites from the Atlas using the atlas set
    public List<Sprite> LoadSpriteSheet()
    {
        List<Sprite> sprites = new List<Sprite>();

        if(m_atlas == null) { Debug.LogError("No atlas found"); return sprites; }
        if(m_atlasSet < 0) { Debug.LogError("Atlas set must be more or equal to 0"); return sprites; }
        if(m_atlasSet >= m_atlas.spriteCount / 8f) { Debug.LogError("Atlas set must be within the range"); return sprites; }

        Sprite[] s = new Sprite[m_atlas.spriteCount];

        m_atlas.GetSprites(s);

        for (int i = m_atlasSet * 8; i < (m_atlasSet * 8) + 8; i++)
            sprites.Add(s[i]);

        sprites = sprites.OrderBy(x => x.name).ToList();

        return sprites;
    }
}

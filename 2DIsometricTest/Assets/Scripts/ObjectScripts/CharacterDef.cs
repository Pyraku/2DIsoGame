using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System.Linq;

[CreateAssetMenu(fileName = "CharacterDef", menuName = "ObjectScripts/CharacterDef", order = 1)]
public class CharacterDef : ScriptableObject
{
    [SerializeField] private string m_key = "";
    public string Key => m_key;

    [SerializeField] private SpriteAtlas m_atlas = null;
    [SerializeField] private int m_atlasSet = 0;

    //Loads the correct sprites from the Atlas using the atlas set
    public List<Sprite> LoadSpriteSheet()
    {
        List<Sprite> sprites = new List<Sprite>();

        if (m_atlas == null) { Debug.LogError("No atlas found"); return sprites; }
        if (m_atlasSet < 0) { Debug.LogError("Atlas set must be more or equal to 0"); return sprites; }
        if (m_atlasSet >= m_atlas.spriteCount / 8f) { Debug.LogError("Atlas set must be within the range"); return sprites; }

        Sprite[] s = new Sprite[m_atlas.spriteCount];

        m_atlas.GetSprites(s);

        for (int i = m_atlasSet * 8; i < (m_atlasSet * 8) + 8; i++)
            sprites.Add(s[i]);

        sprites = sprites.OrderBy(x => x.name).ToList();

        return sprites;
    }
}

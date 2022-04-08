using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Character))]
public class CharacterDebug : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Character wd = (Character)target;

        if (GUILayout.Button("Load Character Def"))
        {
            wd.LoadDef();
        }
    }
}

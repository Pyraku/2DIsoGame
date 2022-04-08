using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Weapon))]
public class WeaponDebug : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Weapon wd = (Weapon)target;

        if (GUILayout.Button("Load Weapon Def"))
        {
            wd.LoadDef();
        }
    }
}

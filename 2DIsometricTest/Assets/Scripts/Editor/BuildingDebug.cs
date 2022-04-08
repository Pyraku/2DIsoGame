using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Building))]
public class BuildingDebug : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Building building = (Building)target;

        if(GUILayout.Button("Place Walls"))
        {
            building.DebugBuildRoom();
        }
    }


}

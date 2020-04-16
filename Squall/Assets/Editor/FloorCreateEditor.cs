using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FloorCreate))]

public class FloorCreateEditor : Editor
{
    /// <summary>
    /// InspectorのGUIを更新
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //targetを変更して対象を取得
        FloorCreate fc = target as FloorCreate;

        if (GUILayout.Button("Create"))
        {
            fc.Create();
        }
    }
}

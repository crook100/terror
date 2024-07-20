using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Ground))]
public class GroundDropDown : Editor
{
    public override void OnInspectorGUI()
    {
        Ground script = (Ground)target;
        GameBrain gb = FindObjectOfType<GameBrain>();
        List<string> groundTypes = new List<string>();

        if (gb.groundTypes.Length <= 0)
        {
            //groundTypes.Add("Add at least one GroundType at Game Brain.");
            //script.groundType = 0;

            if (GUILayout.Button("Add at least one GroundType at Game Brain."))
            {
                Selection.activeObject = gb;
            }
        }
        else {
            foreach (GameBrain.GroundType gt in gb.groundTypes)
            {
                groundTypes.Add(gt.name);
            }
            script.groundType = EditorGUILayout.Popup("Ground Type:", script.groundType, groundTypes.ToArray());
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WorldGenerator))]
public class WorldGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        WorldGenerator mapGen = (WorldGenerator)target;

        if (DrawDefaultInspector ()) {
            if (mapGen.autoUpdate) {
                mapGen.GenerateMap ();
            }
        }

        if (GUILayout.Button ("Generate")) {
            mapGen.GenerateMap ();
        }
    }
}
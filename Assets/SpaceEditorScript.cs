using UnityEngine;
using System.Collections;
using UnityEditor;
using Unity.VisualScripting.FullSerializer;
using System.Collections.Generic;
using UnityEngine.UIElements;

[CustomEditor(typeof(Space))]
public class SpaceEditorScript : Editor
{
    Space myScript;
    List<Transform> potentialneighbours;
    Board parentBoard;
    void OnEnable()
    {
        myScript = (Space)target;
        parentBoard = myScript.GetComponentInParent<Board>();

        potentialneighbours = new List<Transform>();
        if (parentBoard )
        {
            for (int i = 0; i < myScript.transform.parent.childCount; i++)
            {
                potentialneighbours.Add(myScript.transform.parent.GetChild(i));
            }
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Label("Mesh platzieren muss noch ausgebaut werden");
        if (parentBoard) {
            if (parentBoard.SpacePrefab != null)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Mit Mesh verbinden", GUILayout.MaxWidth(200)))
                {
                    myScript.BuildLinesToNeighbours();
                }
                if (GUILayout.Button("Neuer Nachbar", GUILayout.MaxWidth(200), GUILayout.MinWidth(100)))
                {
                    myScript.BuildNeighbour();
                }
                GUILayout.EndHorizontal();
            }
            else {
                EditorGUILayout.HelpBox("Space Prefab in Parent setzen", MessageType.Warning);
            }
        } else {
            EditorGUILayout.HelpBox("Braucht ein Board als Parent", MessageType.Error);
        }

    }

    void OnSceneGUI()
    {
        GUI.skin.label.richText = true;
        foreach (Transform t in potentialneighbours) {
            if (t != null)
            {
                string s = "<b><size=16><color=magenta>" + t.name + "</color></size></b>";
                Handles.Label(t.position,s);
            }
        }
    }

}

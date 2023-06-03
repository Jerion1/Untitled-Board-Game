using UnityEngine;
using System.Collections;
using UnityEditor;
using Unity.VisualScripting.FullSerializer;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Linq;
using Unity.VisualScripting;

[CustomEditor(typeof(Space))]
[CanEditMultipleObjects]
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
        if (Selection.objects.Length > 1)
        {
            if (GUILayout.Button("Selected miteinander verbinden"))
            {
                List<Space> SelectedSpaces = new();
                foreach (Object o in Selection.objects)
                {
                    SelectedSpaces.Add(o.GetComponent<Space>());
                }
                foreach (Space s in SelectedSpaces)
                {
                    foreach (Space s_inner in SelectedSpaces)
                    {
                        if (s_inner != s)
                        {
                            s.neighbourList.Add(s_inner.gameObject);
                        }
                    }
                }
            }
            return;
        }

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
        if (Selection.objects.Length > 1) { return; }
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

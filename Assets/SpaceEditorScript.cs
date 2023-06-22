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
    SerializedProperty neighbours;
    void OnEnable()
    {
        myScript = (Space)target;
        parentBoard = myScript.GetComponentInParent<Board>();

        potentialneighbours = new List<Transform>();
        if (parentBoard)
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
                    SerializedObject so = new SerializedObject(s);
                    neighbours = so.FindProperty("neighbourList");
                    so.Update();
                    foreach (Space s_inner in SelectedSpaces)
                    {
                        if (s_inner != s && !s.neighbourList.Contains(s_inner.gameObject))
                        {
                            neighbours.InsertArrayElementAtIndex(neighbours.arraySize);
                            so.ApplyModifiedProperties();
                            s.neighbourList[s.neighbourList.Count - 1] = s_inner.gameObject;
                            
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
                    //neighbourList von diesem Space "dirty" machen
                    SerializedObject so = new SerializedObject(target);
                    neighbours = so.FindProperty("neighbourList");
                    so.Update();
                    neighbours.InsertArrayElementAtIndex(neighbours.arraySize);
                    neighbours.DeleteArrayElementAtIndex(neighbours.arraySize-1);
                    so.ApplyModifiedProperties();
                    //Nachbar hinzu
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

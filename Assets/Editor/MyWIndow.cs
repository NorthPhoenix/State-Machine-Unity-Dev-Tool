using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class MyWindow : EditorWindow
{
    Vector2 scrollPos;
    List<string> states = new List<string> { "InitialState" };


    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/State Machine Creator")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MyWindow window = (MyWindow)GetWindow(typeof(MyWindow));
        //window.minSize = new Vector2(200, 300); 
        window.Show();
    }

    void OnGUI()
    {
        Rect vRect = EditorGUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins);

        EditorGUILayout.LabelField("States", EditorStyles.boldLabel);
        scrollPos = GUILayout.BeginScrollView(scrollPos, EditorStyles.helpBox);

        for(int i = 0; i < states.Count; i++)
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.inspectorFullWidthMargins);

            states[i] = EditorGUILayout.TextField(states[i]);
            //EditorGUILayout.Space(5);
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_TreeEditor.Trash", "|Delete State"), EditorStyles.iconButton))
            {
                states.RemoveAt(i);
            }

            EditorGUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();

        EditorGUILayout.Space(EditorGUIUtility.singleLineHeight * 0.5f);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add State"))
        {
            states.Add("NewState");
        }
        if (GUILayout.Button("Create")){}

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(EditorGUIUtility.singleLineHeight * 0.5f);

        EditorGUILayout.EndVertical();

    }

}
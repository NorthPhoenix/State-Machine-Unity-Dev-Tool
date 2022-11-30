using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class StateMachineCreatorWindow : EditorWindow
{
    Vector2 scrollPos;
    List<string> states = new List<string>();
    StateMachineCreator SMCreator = new StateMachineCreator();


    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/State Machine Creator")]
    static void ShowWindow()
    {
        // Get existing open window or if none, make a new one:
        StateMachineCreatorWindow window = (StateMachineCreatorWindow)GetWindow(typeof(StateMachineCreatorWindow), false, "State Machine Creator");
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

        if (GUILayout.Button("Add State", GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.5f)))
        {
            states.Add("NewState");
        }
        if (GUILayout.Button("Delete Existing"))
        {
            SMCreator.DeleteExisting();
        }
        if (GUILayout.Button("Create"))
        {
            SMCreator.Run(states);
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(EditorGUIUtility.singleLineHeight * 0.5f);

        EditorGUILayout.EndVertical();

    }

}
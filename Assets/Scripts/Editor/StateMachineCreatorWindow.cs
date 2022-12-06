using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Globalization;
using System;

public class StateMachineCreatorWindow : EditorWindow
{
    Vector2 scrollPos;
    List<string> states = new List<string>();
    StateMachineCreator SMCreator;

    private bool notEnoughStates = false;
    private bool sameNames = false;
    private bool emptyName = false;
    private bool startsWithDigit = false;

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
        EditorGUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins);

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("States", EditorStyles.boldLabel, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.7f));
        if (GUILayout.Button("Validate States", GUILayout.MinWidth(100)))
        {
            Validate();
        }

        EditorGUILayout.EndHorizontal();

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
            if (notEnoughStates)
            {
                notEnoughStates = false;
            }
            states.Add("New State");
        }
        if (GUILayout.Button(new GUIContent("Delete Existing", "Deletes the root folder for this tool")))
        {
            if (SMCreator is not null)
            {
                SMCreator.DeleteExisting();
            }
        }
        if (GUILayout.Button("Create"))
        {
            if (Validate())
            {
                SMCreator = new StateMachineCreator();
                SMCreator.Run(states);
            }
        }

        EditorGUILayout.EndHorizontal();

        if (notEnoughStates)
        {
            EditorGUILayout.HelpBox("Add at least one state.", MessageType.Error, true);
        }
        if (sameNames)
        {
            EditorGUILayout.HelpBox("States cannot have same names.", MessageType.Error, true);
        }
        if (emptyName)
        {
            EditorGUILayout.HelpBox("A state name cannot be empty.", MessageType.Error, true);
        }
        if (startsWithDigit)
        {
            EditorGUILayout.HelpBox("State name cannot start with a digit.", MessageType.Error, true);
        }
        EditorGUILayout.Space(EditorGUIUtility.singleLineHeight * 0.5f);

        EditorGUILayout.EndVertical();

    }


    private bool Validate()
    {
        //Make sure that there is at least one state added
        if (!(states.Count > 0))
        {
            notEnoughStates = true;
            return false;
        }

        //Fixup state names
        for (int i = 0; i < states.Count; i++)
        {
            states[i] = states[i].Trim();
            if (states[i].Length == 0)
            {
                emptyName = true;
                return false;
            }
            states[i] = ToTitleCase(states[i]);
        }
        emptyName = false;

        //Make sure no state names start with a number
        for (int i = 0; i < states.Count; i++)
        {
            if (Char.IsDigit(states[i][0]))
            {
                startsWithDigit = true;
                return false;
            }
        }
        startsWithDigit = false;

        //Make sure that all the state names are unique
        List<string> temp = new List<string>();
        for (int i = 0; i < states.Count; i++)
        {
            temp.Add(states[i].ToUpper().Replace(" ", ""));
        }
        for (int i = 0; i < temp.Count; i++)
        {
            for (int j = i + 1; j < temp.Count; j++)
            {
                if (temp[i].Equals(temp[j]))
                {
                    sameNames = true;
                    return false;
                }
            }
        }
        sameNames = false;
        return true;
    }

    // Transform passed string to title case and return it
    // also cuts out repeating whitespace
    private string ToTitleCase(string str)
    {
        if (char.IsLetter(str[0]))
        {
            str = str[0].ToString().ToUpper() + str[1..];
        }
        if(str.Length < 2)
        {
            return str;
        }

        char first;
        char second;
        int i = 1;
        while (i < str.Length)
        {
            first = str[i - 1];
            second = str[i];
            if (!char.IsWhiteSpace(first))
            {
                i++;
                continue;
            }
            if(char.IsLetter(second))
            {
                str = str.Remove(i, 1);
                str = str.Insert(i, second.ToString().ToUpper());
                i += 2;
            }
            else if (char.IsWhiteSpace(second))
            {
                str = str.Remove(i, 1);
            }
            else
            {
                i++;
            }
        }
        return str;
    }
}
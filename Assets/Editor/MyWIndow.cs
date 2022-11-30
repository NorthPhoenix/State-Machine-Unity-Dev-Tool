using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class MyWindow : EditorWindow
{
    List<State> states;
    float padding = 10f;
    Vector2 scrollPos;


    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/State Machine Creator")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MyWindow window = (MyWindow)GetWindow(typeof(MyWindow));
        window.minSize = new Vector2(500, 300); 
        window.Show();
    }

    void OnGUI()
    {

        GUILayout.BeginScrollView(scrollPos, EditorStyles.helpBox, GUILayout.Width(150), GUILayout.Height(position.height));

        GUILayout.Button("Click me");
        GUILayout.EndScrollView();

    }

}
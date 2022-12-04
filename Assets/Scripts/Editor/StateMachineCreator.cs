using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class StateMachineCreator
{
    public void Run(List<string> states)
    {
        GenFolder();
        GenFactory(states);
        GenBaseState();
        GenStates(states);
        GenMachine();
    }

    public void DeleteExisting()
    {
        // Delete folder on path: "Assets/State Machine Creator
        if (AssetDatabase.IsValidFolder("Assets/State Machine Creator"))
        {
            AssetDatabase.DeleteAsset("Assets/State Machine Creator");
        }
    }

    private void GenFolder()
    {
        // Create path: "Assets/State Machine Creator/State Machine
        if (!AssetDatabase.IsValidFolder("Assets/State Machine Creator"))
        {
            AssetDatabase.CreateFolder("Assets", "State Machine Creator");
        }
        AssetDatabase.CreateFolder("Assets/State Machine Creator", "State Machine");
    }

    private void GenFactory(List<string> states)
    {
        string factoryCode = 
 @"using System.Collections.Generic;

// States Enum
enum States
{
    ";
        factoryCode += states[0].Trim().ToUpper().Replace(' ', '_');
        for (int i = 1; i < states.Count; i++)
        {
            factoryCode += ",\n    ";
            factoryCode += states[i].Trim().ToUpper().Replace(' ', '_');
        }
        Debug.Log(factoryCode);
}

private void GenBaseState()
    {
        return;
    }

    private void GenStates(List<string> states)
    {
        return;
    }

    private void GenMachine()
    {
        return;
    }
}

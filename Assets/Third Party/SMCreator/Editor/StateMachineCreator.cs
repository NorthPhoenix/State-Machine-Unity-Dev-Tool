using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class StateMachineCreator
{
    #region String Templates

    #region State Factory
    private static string FactoryCodeTemplate =
@"using System.Collections.Generic;

// States Enum
enum States
{{
{0}
}}

public class StateFactory
{{
    StateMachine _context;
    Dictionary<States, BaseState> _states = new Dictionary<States, BaseState>();

    public StateFactory(StateMachine currentContext) {{
        _context = currentContext;
        //_states[States.SOME_STATE_NAME1] = new SomeState(_context, this);
{1}        
    }}

    /*
     *public BaseState StateName1() {{
     *    return _states[States.SOME_STATE_NAME1];
     *}}
     */
{2}
}}";

    private static string DictTemplate =
@"_states[States.{0}] = new {1}(_context, this);";

    private static string MethodTemplate =
@"    public BaseState {0}() {{
        return _states[States.{1}];
    }}";
    #endregion

    #region Base State
    private static string BaseStateCode =
@"public abstract class BaseState
{
    private StateMachine _ctx;
    private StateFactory _factory;

    protected StateMachine Ctx { get { return _ctx; } }
    protected StateFactory Factory { get { return _factory; } }

    public BaseState(StateMachine currentContext, StateFactory factory)
    {
        _ctx = currentContext;
        _factory = factory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchState();

    protected void SwitchState(BaseState newState)
    {
        //current states exits
        ExitState();

        //new state enters state
        newState.EnterState();
        _ctx.CurrentState = newState;
    }
}";
    #endregion

    #region Individual State
    private static string StateTemplate =
@"using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class {0} : BaseState
{{
    private static string _stateName = ""{0}"";

    //Constructor
    public {0}(StateMachine currentContext, StateFactory factory)
        : base(currentContext, factory) {{ }}

    // Method Overrides
    public override void EnterState()
    {{
        Debug.Log($""Enter {{_stateName}}"");
    }}

    public override void UpdateState()
    {{
        Debug.Log($""Update {{_stateName}}"");
        // --> Update state logic here <--

        CheckSwitchState(); //execute after logic update
    }}

    public override void ExitState()
    {{
        Debug.Log($""Exit {{_stateName}}"");
    }}

    public override void CheckSwitchState()
    {{
        /* ---TEMPLATE---
        if (newStateCondition == true)
        {{
            Debug.Log(""Switching to NewState"");
            SwitchState(Factory.NewState()); //Call factory method for new state
        }}
        */
    }}
}}";
    #endregion

    #region State Machine
    private static string MachineCode =
@"using UnityEngine;

public class StateMachine : MonoBehaviour
{
    // variables
    //[SerializeField] type _variableName; <-- template

    // state variables
    BaseState _currentState;
    StateFactory _states;


    //getters and setters
    public BaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    //public type VariableName { get { return _variableName; } set { _variableName = value; } } <-- template


    void Awake()
    {
        //state setup

        //Initialize state factory
        _states = new StateFactory(this);

        // TODO - Set the starting state
        //_currentState = _states.StateName(); <-- template

        // Enter the starting state
        _currentState.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        // Update the current state
        _currentState.UpdateState();
    }
}";

    #endregion

    #endregion

    private string SMFolder;

    private int stateCount = 0;
    private List<string> enumStr = new List<string>();
    private List<string> methodStr = new List<string>();

    #region Public Functions
    public void Run(List<string> states)
    {
        GenFolder();
        GenStateNames(states);
        GenFactory();
        GenBaseState();
        GenStates();
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
    #endregion

    #region Private Generation Functions
    private void GenStateNames(List<string> states)
    {
        for (int i = 0; i < states.Count; i++)
        {
            enumStr.Add(states[i].ToUpper().Replace(' ', '_'));
            methodStr.Add(states[i].Replace(" ",""));
            stateCount++;
        }
        #region Debug.Log
        //Debug.Log
        //string print = $"{states[0]}";
        //for (int i = 1; i < states.Count; i++)
        //{
        //    print += $", {states[i]}";
        //}
        //Debug.Log($"states:\n{print}");

        //print = $"{enumStr[0]}";
        //for (int i = 1; i < enumStr.Count; i++)
        //{
        //    print += $", {enumStr[i]}";
        //}
        //Debug.Log($"enumStr:\n{print}");

        //print = $"{methodStr[0]}";
        //for (int i = 1; i < methodStr.Count; i++)
        //{
        //    print += $", {methodStr[i]}";
        //}
        //Debug.Log($"methodStr:\n{print}");
        #endregion
    }

    private void GenFolder()
    {
        // Create path: "Assets/State Machine Creator/State Machine
        if (!AssetDatabase.IsValidFolder("Assets/State Machine Creator"))
        {
            AssetDatabase.CreateFolder("Assets", "State Machine Creator");
        }
        SMFolder = Path.GetFileName(AssetDatabase.GUIDToAssetPath(AssetDatabase.CreateFolder("Assets/State Machine Creator", "State Machine")));
    }

    private void GenFactory()
    {
        string statesEnum = "";
        string sectionIndent = "    ";
        statesEnum += sectionIndent;
        statesEnum += enumStr[0];
        for (int i = 1; i < stateCount; i++)
        {
            statesEnum += $",\r\n{sectionIndent}";
            statesEnum += enumStr[i];
        }
        //Debug.Log($"stateEnum:\n{statesEnum}");

        string statesDict = "";
        sectionIndent = "        ";
        statesDict += sectionIndent;
        statesDict += String.Format(DictTemplate, enumStr[0], methodStr[0]);
        for (int i = 1; i < stateCount; i++)
        {
            statesDict += $"\r\n{sectionIndent}";
            statesDict += String.Format(DictTemplate, enumStr[i], methodStr[i]);
        }
        //Debug.Log($"statesDict:\n{statesDict}");

        string statesFunc = "";
        statesFunc += String.Format(MethodTemplate, methodStr[0], enumStr[0]);
        for (int i = 1; i < stateCount; i++)
        {
            statesFunc += "\r\n\r\n";
            statesFunc += String.Format(MethodTemplate, methodStr[i], enumStr[i]);
        }
        //Debug.Log($"statesFunc:\n{statesFunc}");

        string statesFactory = String.Format(FactoryCodeTemplate, statesEnum, statesDict, statesFunc);
        //Debug.Log($"statesFactory:\n{statesFactory}");

        string relativePath = $"Assets/State Machine Creator/{SMFolder}/StateFactory.cs";
        string fullPath = $"{Application.dataPath}/State Machine Creator/{SMFolder}/StateFactory.cs";
        File.WriteAllText(fullPath, statesFactory);
        AssetDatabase.ImportAsset(relativePath);
    }

    private void GenBaseState()
    {
        string relativePath = $"Assets/State Machine Creator/{SMFolder}/BaseState.cs";
        string fullPath = $"{Application.dataPath}/State Machine Creator/{SMFolder}/BaseState.cs";
        File.WriteAllText(fullPath, BaseStateCode);
        AssetDatabase.ImportAsset(relativePath);
    }

    private void GenStates()
    {
        for (int i = 0; i < stateCount; i++)
        {
            GenState(methodStr[i]);
        }
    }

    private void GenState(string titleName)
    {
        string stateCode = String.Format(StateTemplate, titleName);

        string relativePath = $"Assets/State Machine Creator/{SMFolder}/{titleName}.cs";
        string fullPath = $"{Application.dataPath}/State Machine Creator/{SMFolder}/{titleName}.cs";
        File.WriteAllText(fullPath, stateCode);
        AssetDatabase.ImportAsset(relativePath);
    }

    private void GenMachine()
    {
        string relativePath = $"Assets/State Machine Creator/{SMFolder}/StateMachine.cs";
        string fullPath = $"{Application.dataPath}/State Machine Creator/{SMFolder}/StateMachine.cs";
        File.WriteAllText(fullPath, MachineCode);
        AssetDatabase.ImportAsset(relativePath);
    }
    #endregion
}

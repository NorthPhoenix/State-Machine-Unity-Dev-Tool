using System.Collections.Generic;

public struct State
{
    public string name;
    public List<State> transitionsTo;
}

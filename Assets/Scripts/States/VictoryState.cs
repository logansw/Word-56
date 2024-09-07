using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryState : State
{
    public VictoryState()
    {
        GameState = StateType.Victory;
    }

    public override void OnEnter(StateController stateController)
    {
        // Do nothing
    }

    public override void UpdateState(StateController stateController)
    {
        // Do nothing
    }

    public override void OnExit(StateController stateController)
    {
        // Do nothing
    }
}

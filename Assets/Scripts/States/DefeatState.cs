using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatState : State
{
    public DefeatState()
    {
        GameState = StateType.Defeat;
    }

    public override void OnEnter(StateController stateController)
    {
        GameManager.s_instance.SetActivePanel(3);
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

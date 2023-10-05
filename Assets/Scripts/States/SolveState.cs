using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolveState : State
{
    public SolveState() {
        GameState = StateType.Solve;
    }
    public override void OnEnter(StateController stateController)
    {
        // Show back button
    }
    public override void UpdateState(StateController stateController)
    {
        // On cancel button pressed
        // On submit button pressed
            // Return if wrong
            // GameOver if correct
    }
    public override void OnExit(StateController stateController)
    {
        // Hide back button
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : State
{
    public GameOverState() {
        GameState = StateType.GameOver;
    }
    public override void OnEnter(StateController stateController)
    {
        // Show end screen
        // If more games remain, show continue button
        // If final game, show breakdown and quit button
    }
    public override void UpdateState(StateController stateController)
    {
        // On continue button or quit button pressed
    }
    public override void OnExit(StateController stateController)
    {
        // Close end screen
        // If rounds remain, prepare accordingly
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondChanceState : State
{
    public static Action e_OnSecondChanceAsk; // Triggered upon entering second chance screen

    public SecondChanceState()
    {
        GameState = StateType.SecondChance;
    }

    public override void OnEnter(StateController stateController)
    {
        GameManager.s_instance.SetActivePanel(1);
    }

    public override void UpdateState(StateController stateController)
    {
        // Do nothing
    }

    public override void OnExit(StateController stateController)
    {
        Letter.ResetCooldowns = true;
    }
}

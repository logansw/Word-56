using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatState : State
{
    public static Action e_OnDefeat;

    public DefeatState()
    {
        GameState = StateType.Defeat;
    }

    public override void OnEnter(StateController stateController)
    {
        if (!WordManager.s_instance.IsSecondChance)
        {
            GameManager.s_instance.SetActivePanel(2);
        }
        else
        {
            HighscoreWriter.s_Instance.CheckOnLeaderboard();
        }
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

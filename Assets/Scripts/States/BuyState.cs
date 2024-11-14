using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyState : State
{
    public BuyState() {
        GameState = StateType.Buy;
    }
    public static Action e_OnBuyStateEntered;
    public override void OnEnter(StateController stateController)
    {
        e_OnBuyStateEntered?.Invoke();
    }
    public override void UpdateState(StateController stateController)
    {
        // On letter pressed, advance the round. Re-enter BuyState.
        // stateController.ChangeState(stateController.BuyState);
    }
    public override void OnExit(StateController stateController)
    {
        // Hide prices
    }
}
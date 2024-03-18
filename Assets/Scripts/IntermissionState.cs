using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntermissionState : State
{
    [SerializeField] private GameObject _screenIntermission;
    [SerializeField] private GameObject _screenMain;

    public IntermissionState() {
        GameState = StateType.Intermission;
    }
    public override void OnEnter(StateController stateController)
    {
        _screenMain.SetActive(false);
        _screenIntermission.SetActive(true);
    }
    public override void UpdateState(StateController stateController)
    {
        // Nothing
    }
    public override void OnExit(StateController stateController)
    {
        ScoreBreakdown scoreBreakdown = FindObjectOfType<ScoreBreakdown>();
        if (scoreBreakdown != null)
            Destroy(scoreBreakdown.gameObject);
        _screenIntermission.SetActive(false);
    }
}

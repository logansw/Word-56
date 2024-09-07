using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntermissionState : State
{
    [SerializeField] private GameObject _screenIntermission;
    [SerializeField] private GameObject _screenMain;
    [SerializeField] private Button[] _buttonsToDisable;
    [SerializeField] private Button[] _buttonsToEnable;
    [SerializeField] private Button _continueButton;

    public IntermissionState() {
        GameState = StateType.Intermission;
    }
    public override void OnEnter(StateController stateController)
    {
        _screenMain.SetActive(false);
        _screenIntermission.SetActive(true);
        foreach (Button button in _buttonsToDisable)
        {
            button.gameObject.SetActive(false);
        }
        foreach (Button button in _buttonsToEnable)
        {
            button.gameObject.SetActive(true);
        }
        if (GameManager.s_instance.IsLastRound())
        {
            _continueButton.onClick.AddListener(() => GameManager.s_instance.Finish());
        }
    }
    public override void UpdateState(StateController stateController)
    {
        // Nothing
    }
    public override void OnExit(StateController stateController)
    {
        _screenIntermission.SetActive(false);
        foreach (Button button in _buttonsToDisable) {
            button.gameObject.SetActive(true);
        }
        foreach (Button button in _buttonsToEnable) {
            button.gameObject.SetActive(false);
        }
    }
}
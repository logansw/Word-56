using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SolveState : State
{
    [SerializeField] private Button _solveButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private Button _deleteButton;
    [SerializeField] private Button _enterButton;
    [SerializeField] private Button _undoAllButton;

    public SolveState() {
        GameState = StateType.Solve;
    }
    public override void OnEnter(StateController stateController)
    {
        _solveButton.interactable = false;
        _cancelButton.gameObject.SetActive(true);
        // _deleteButton.gameObject.SetActive(true);
        _enterButton.gameObject.SetActive(true);
        _undoAllButton.gameObject.SetActive(true);
        WordManager.s_instance.RenderEnterButton();
        if (WordManager.s_instance.GetCurrentGuess().Length == 11)
        {
            WordManager.s_instance.SubmitSolveAttempt();
        }
        Camera.main.backgroundColor = new Color(0.6f, 0.8812134f, 0.9058824f);
    }
    public override void UpdateState(StateController stateController)
    {
        WordManager.s_instance.RenderEnterButton();
    }
    public override void OnExit(StateController stateController)
    {
        _solveButton.interactable = true;
        _cancelButton.gameObject.SetActive(false);
        _deleteButton.gameObject.SetActive(false);
        _enterButton.gameObject.SetActive(false);
        _undoAllButton.gameObject.SetActive(false);
        WordManager.s_instance.CancelSolveAttempt();
        Camera.main.backgroundColor = new Color(0.9058824f, 0.7215686f, 0.6f);
    }
}
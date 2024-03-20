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
        _deleteButton.gameObject.SetActive(true);
        _enterButton.gameObject.SetActive(true);
        _undoAllButton.gameObject.SetActive(true);
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
        _solveButton.interactable = true;
        _cancelButton.gameObject.SetActive(false);
        _deleteButton.gameObject.SetActive(false);
        _enterButton.gameObject.SetActive(false);
        _undoAllButton.gameObject.SetActive(false);
    }
}
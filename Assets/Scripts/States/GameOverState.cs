using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverState : State
{
    [SerializeField] private TMP_Text _finalScoreText;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _finishPanel;

    public GameOverState() {
        GameState = StateType.GameOver;
    }
    public override void OnEnter(StateController stateController)
    {
        GameManager.s_instance.SetActivePanel(1);
    }

    public override void UpdateState(StateController stateController)
    {
        // On continue button or quit button pressed
    }
    public override void OnExit(StateController stateController)
    {
        GameManager.s_instance.SetActivePanel(0);
        // If rounds remain, prepare accordingly
    }
}

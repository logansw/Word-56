using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverState : State
{
    [SerializeField] private GameObject _screenGameOver;
    [SerializeField] private TMP_Text _finalScoreText;
    [SerializeField] private ScoreBreakdown _scoreBreakdownPrefab;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _finishPanel;

    public GameOverState() {
        GameState = StateType.GameOver;
    }
    public override void OnEnter(StateController stateController)
    {
        _screenGameOver.SetActive(true);
        RenderBreakdowns();
        _finalScoreText.text = "Final Score: " + GameManager.s_instance.GetFinalScore();
    }

    private void RenderBreakdowns() {
        int seriesLength = ConfigurationManager.s_instance.SeriesLength;
        float width = _canvas.pixelRect.width / seriesLength * 0.9f;
        width = Mathf.Min(width, _canvas.pixelRect.width / 2f);
        for (int i = 0; i < seriesLength; i++) {
            ScoreBreakdown scoreBreakdown = Instantiate(_scoreBreakdownPrefab, _finishPanel.transform);
            scoreBreakdown.ScaleToWidth(width);
            scoreBreakdown.transform.localPosition = new Vector3(-(_canvas.pixelRect.width / 2f * 0.9f) + (i * width) + width / 2f, 80, 0);
            int roundNumber = i + 1;
            int letterPurchases = GameManager.s_instance.LetterPurchases[i];
            int timeElapsed = GameManager.s_instance.TimeElapses[i];
            int solvePurchases = GameManager.s_instance.SolvePurchases[i];
            scoreBreakdown.Initialize(roundNumber, letterPurchases, timeElapsed, solvePurchases);
        }
    }

    public override void UpdateState(StateController stateController)
    {
        // On continue button or quit button pressed
    }
    public override void OnExit(StateController stateController)
    {
        _screenGameOver.SetActive(false);
        // If rounds remain, prepare accordingly
    }
}

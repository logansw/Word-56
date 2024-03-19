using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreBreakdown : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private TMP_Text _roundNumberText;
    [SerializeField] private TMP_Text _startingScoreText;
    [SerializeField] private TMP_Text _totalScoreText;
    [SerializeField] private TMP_Text _letterPurchasesText;
    [SerializeField] private TMP_Text _timeElapsedText;
    [SerializeField] private TMP_Text _solvePurchasesText;

    public void Initialize(int roundNumber, int letterPurchases, int timeElapsed, int solvePurchases) {
        _roundNumberText.text = "Round " + roundNumber;
        _startingScoreText.text = "Starting Score: " + ConfigurationManager.s_instance.StartingScore;
        _letterPurchasesText.text = "Letter Purchases: -" + letterPurchases.ToString();
        _timeElapsedText.text = "Time Elapsed: " + timeElapsed.ToString() + "s";
        _solvePurchasesText.text = "Solve Purchases: -" + solvePurchases.ToString();
        _totalScoreText.text = "Round Total: " + WordManager.s_instance.RoundScore;
    }

    public void ScaleToWidth(float width) {
        float ratio = width / _rectTransform.rect.width;
        _rectTransform.localScale = new Vector3(ratio, ratio, 1);
    }
}

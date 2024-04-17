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
    [SerializeField] private TMP_Text _timeBonusText;
    [SerializeField] private TMP_Text _selectionBonusText;

    public void Initialize(int roundNumber, int letterPurchases, int timeElapsed, int solvePurchases, int timeBonus, int selectionBonus) {
        _roundNumberText.text = "Round " + roundNumber;
        _startingScoreText.text = "Starting Score: " + ConfigurationManager.s_instance.StartingScore;
        _letterPurchasesText.text = "Letter Purchases: -" + letterPurchases.ToString();
        _timeElapsedText.text = "Time Elapsed: " + timeElapsed.ToString() + "s";
        _solvePurchasesText.text = "Solve Purchases: -" + solvePurchases.ToString();
        _timeBonusText.text = "Time Bonus: " + timeBonus.ToString();
        _selectionBonusText.text = "Selection Bonus: " + selectionBonus.ToString();
        _totalScoreText.text = "Round Total: " + (ConfigurationManager.s_instance.StartingScore - letterPurchases - solvePurchases).ToString();
    }

    public void ScaleToWidth(float width) {
        float ratio = width / _rectTransform.rect.width;
        _rectTransform.localScale = new Vector3(ratio, ratio, 1);
    }
}

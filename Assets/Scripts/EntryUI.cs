using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EntryUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _cleanWins;
    [SerializeField] private TMP_Text _retryWins;
    [SerializeField] private TMP_Text _cleanLosses;
    [SerializeField] private TMP_Text _retryLosses;
    [SerializeField] private TMP_Text _averageScore;
    [SerializeField] private TMP_Text _totalScore;
    [SerializeField] private TMP_Text _totalGames;
    [SerializeField] private Image _backgroundImage;

    public void SetEntry(HighscoreData.Entry entry)
    {
        _name.text = entry.Name;
        _cleanWins.text = entry.CleanWins.ToString();
        _retryWins.text = entry.RetryWins.ToString();
        _cleanLosses.text = entry.CleanLosses.ToString();
        _retryLosses.text = entry.RetryLosses.ToString();
        _averageScore.text = entry.AverageScore.ToString();
        _totalScore.text = entry.TotalScore.ToString();
        _totalGames.text = entry.TotalGames.ToString();
    }

    public void SetColor(Color color)
    {
        _backgroundImage.color = color;
    }
}
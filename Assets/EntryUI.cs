using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EntryUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _rank;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _date;
    [SerializeField] private TMP_Text _score;
    [SerializeField] private Image _backgroundImage;

    public void SetEntry(HighscoreData.Entry entry, int rank) {
        _rank.text = rank.ToString();
        _name.text = entry.Name;
        _date.text = $"{entry.Month.ToString("00")}/{entry.Day.ToString("00")}/{entry.Year.ToString("0000")}";
        _score.text = entry.Score.ToString();
    }

    public void SetColor(Color color) {
        _rank.color = color;
        _backgroundImage.color = color;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class HighscoreWriter : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private GameObject _addHighscorePanel;
    [SerializeField] private GameObject _finishPanel;

    public void CheckOnLeaderboard() {
        bool regular = !ConfigurationManager.s_instance.ChallengeMode;
        int seriesLength = ConfigurationManager.s_instance.SeriesLength;
        if (OnLeaderboard(GameManager.s_instance.GetFinalScore(), regular, seriesLength)) {
            _finishPanel.SetActive(false);
            _addHighscorePanel.SetActive(true);
            if (PlayerPrefs.HasKey("PreviousName")) {
                _nameInputField.text = PlayerPrefs.GetString("PreviousName");
            }
        } else {
            SceneManager.LoadScene("Title");
        }
    }

    public bool OnLeaderboard(int newScore, bool regular, int seriesLength) {
        HighscoreData highscoreData = GetHighscoreData(regular, seriesLength);
        return OnTheLeaderboard(highscoreData.Highscores, newScore);
    }

    private bool OnTheLeaderboard(List<HighscoreData.Entry> highscores, int newScore) {
        if (highscores.Count < HighscoreManager.MAX_ENTRIES) {
            return true;
        }
        foreach (HighscoreData.Entry entry in highscores) {
            if (newScore > entry.Score) {
                return true;
            }
        }
        return false;
    }

    public void SaveHighscore() {
        WriteHighscore(GameManager.s_instance.GetFinalScore());
        SceneManager.LoadScene("Title");
    }

    public void WriteHighscore(int score) {
        if (_nameInputField.text.Length == 0) {
            _nameInputField.text = "Anonymous";
        }
        PlayerPrefs.SetString("PreviousName", _nameInputField.text);
        bool regular = !ConfigurationManager.s_instance.ChallengeMode;
        int seriesLength = ConfigurationManager.s_instance.SeriesLength;
        HighscoreData highscoreData = GetHighscoreData(regular, seriesLength);
        highscoreData.Highscores.Add(new HighscoreData.Entry(_nameInputField.text, score, DateTime.Now));
        highscoreData.Highscores = SortAndTruncateHighscores(highscoreData.Highscores);

        string path = GetHighscoreDataPath(regular, seriesLength);
        JSONTool.WriteData<HighscoreData>(highscoreData, path);
    }

    private List<HighscoreData.Entry> SortAndTruncateHighscores(List<HighscoreData.Entry> highscores) {
        List<HighscoreData.Entry> highscoresTruncated = new List<HighscoreData.Entry>();
        highscores.Sort((x, y) => y.CompareTo(x));
        for (int i = 0; i < HighscoreManager.MAX_ENTRIES; i++) {
            if (i >= highscores.Count) {
                break;
            }
            highscoresTruncated.Add(highscores[i]);
        }
        return highscoresTruncated;
    }

    private HighscoreData GetHighscoreData(bool regular, int seriesLength) {
        if (regular) {
            switch (seriesLength) {
                case 1:
                    return JSONTool.ReadData<HighscoreData>("_regularSingleScores.json");
                case 3:
                    return JSONTool.ReadData<HighscoreData>("_regularTripleScores.json");
                case 5:
                    return JSONTool.ReadData<HighscoreData>("_regularPentaScores.json");
            }
        } else {
            switch (seriesLength) {
                case 1:
                    return JSONTool.ReadData<HighscoreData>("_challengeSingleScores.json");
                case 3:
                    return JSONTool.ReadData<HighscoreData>("_challengeTripleScores.json");
                case 5:
                    return JSONTool.ReadData<HighscoreData>("_challengePentaScores.json");
            }
        }
        return null;
    }

    private string GetHighscoreDataPath(bool regular, int seriesLength) {
        if (regular) {
            switch (seriesLength) {
                case 1:
                    return "_regularSingleScores.json";
                case 3:
                    return "_regularTripleScores.json";
                case 5:
                    return "_regularPentaScores.json";
            }
        } else {
            switch (seriesLength) {
                case 1:
                    return "_challengeSingleScores.json";
                case 3:
                    return "_challengeTripleScores.json";
                case 5:
                    return "_challengePentaScores.json";
            }
        }
        return null;
    }
}

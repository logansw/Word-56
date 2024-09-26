using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class HighscoreWriter : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private GameObject _addHighscorePanel;
    [SerializeField] private GameObject _finishPanel;

    // TODO: Change so that it always requires you to record the score, but still asks for your name
    public void CheckOnLeaderboard()
    {
        bool regular = !ConfigurationManager.s_instance.ChallengeMode;
        int seriesLength = ConfigurationManager.s_instance.SeriesLength;
        // if (OnLeaderboard(GameManager.s_instance.GetFinalScore(), regular, seriesLength))
        // {
        //     _finishPanel.SetActive(false);
        //     _addHighscorePanel.SetActive(true);
        //     if (PlayerPrefs.HasKey("PreviousName"))
        // {
    //         _nameInputField.text = PlayerPrefs.GetString("PreviousName");
    //     }
        // }
        // else
        // {
        //     SceneManager.LoadScene("Title");
        // }
        SceneManager.LoadScene("Title");
    }

    public void SaveHighscore()
    {
        // TODO: Write outcome here
        SceneManager.LoadScene("Title");
    }

    public void SkipHighscore()
    {
        SceneManager.LoadScene("Title");
    }

    public void WriteHighscore(int score)
    {
        if (_nameInputField.text.Length == 0)
        {
            _nameInputField.text = "Anonymous";
        }
        PlayerPrefs.SetString("PreviousName", _nameInputField.text);
        // TODO: 9.26.2024 - Add daily/endless distinction here
        bool daily = false;
        HighscoreData highscoreData = GetHighscoreData(daily);
        List<HighscoreData.Entry> entries = highscoreData.Highscores;
        HighscoreData.Entry targetEntry = new HighscoreData.Entry(_nameInputField.text, 0, 0, 0, 0);
        for (int i = 0; i < entries.Count; i++)
        {
            HighscoreData.Entry entry = entries[i];
            if (entry.Name == _nameInputField.text)
            {
                targetEntry = entry;
                entries.Remove(entry);
                break;
            }
        }

        // TODO: 9.26.2024 - Update based on outcome
        highscoreData.Highscores.Add(targetEntry);
        highscoreData.Highscores = SortAndTruncateHighscores(highscoreData.Highscores);

        string path = daily ? "_dailyScores.json" : "_endlessScores.json";
        JSONTool.WriteData<HighscoreData>(highscoreData, path);
    }

    private List<HighscoreData.Entry> SortAndTruncateHighscores(List<HighscoreData.Entry> highscores)
    {
        List<HighscoreData.Entry> highscoresTruncated = new List<HighscoreData.Entry>();
        highscores.Sort((x, y) => y.CompareTo(x));
        for (int i = 0; i < HighscoreManager.MAX_ENTRIES; i++)
        {
            if (i >= highscores.Count)
            {
                break;
            }
            highscoresTruncated.Add(highscores[i]);
        }
        return highscoresTruncated;
    }

    private HighscoreData GetHighscoreData(bool daily)
    {
        if (daily)
        {
            return JSONTool.ReadData<HighscoreData>("_dailyScores.json");
        }
        else
        {
            return JSONTool.ReadData<HighscoreData>("_endlessScores.json");
        }
    }
}

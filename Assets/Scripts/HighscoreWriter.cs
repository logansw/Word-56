using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighscoreWriter : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private GameObject _addHighscorePanel;
    [SerializeField] private GameObject _mainPanel;
    public static HighscoreWriter s_Instance;
    [SerializeField] private TMP_Text _outcomeText;

    void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CheckOnLeaderboard()
    {
        if (PlayerPrefs.HasKey("PreviousName"))
        {
            _nameInputField.text = PlayerPrefs.GetString("PreviousName");
        }
    }

    public void ShowHighscoreEntry()
    {
        _mainPanel.SetActive(false);
        _addHighscorePanel.SetActive(true);
        WordManager.s_instance.FreezeGame();
    }

    public void SaveHighscore()
    {
        if (_nameInputField.text.Length == 0)
        {
            _nameInputField.text = "Anonymous";
        }
        PlayerPrefs.SetString("PreviousName", _nameInputField.text);
        HighscoreData highscoreData = GetHighscoreData(ConfigurationManager.s_instance.CurrentLevel);
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

        if (WordManager.s_instance.Victory)
        {
            if (WordManager.s_instance.IsSecondChance)
            {
                targetEntry.RetryWins = targetEntry.RetryWins + 1;
            }
            else
            {
                targetEntry.CleanWins = targetEntry.CleanWins + 1;
            }
        }
        else 
        {
            if (WordManager.s_instance.IsSecondChance)
            {
                targetEntry.RetryLosses = targetEntry.RetryLosses + 1;
            }
            else
            {
                targetEntry.CleanLosses = targetEntry.CleanLosses + 1;
            }
        }
        targetEntry.CalculateStats();
        highscoreData.Highscores.Add(targetEntry);
        highscoreData.Highscores = SortAndTruncateHighscores(highscoreData.Highscores);

        string path = $"Difficulty{ConfigurationManager.s_instance.CurrentLevel}Scores.json";
        JSONTool.WriteData<HighscoreData>(highscoreData, path);
        SceneManager.LoadScene("Title");
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

    private HighscoreData GetHighscoreData(int difficultyLevel)
    {
        return JSONTool.ReadData<HighscoreData>($"Difficulty{difficultyLevel}Scores.json");
    }

    public void SetOutcomeText(bool victory, bool secondChance)
    {
        if (victory && !secondChance)
        {
            _outcomeText.text = $"Victory! (+10)";
        }
        else if (victory && secondChance)
        {
            _outcomeText.text = $"Comeback! (+4)";
        }
        else if (!victory && !secondChance)
        {
            _outcomeText.text = $"Conceded (+1.5)";
        }
        else
        {
            _outcomeText.text = $"Better luck next time (+0)";
        }
    }
}

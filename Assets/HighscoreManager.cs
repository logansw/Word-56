using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using TMPro;
using UnityEngine.SceneManagement;

public class HighscoreManager : MonoBehaviour
{
    public static HighscoreManager s_instance;
    public const int MAX_ENTRIES = 40;
    private HighscoreData _regularSingleScores;
    private HighscoreData _regularTripleScores;
    private HighscoreData _regularPentaScores;
    private HighscoreData _challengeSingleScores;
    private HighscoreData _challengeTripleScores;
    private HighscoreData _challengePentaScores;
    [SerializeField] private EntryUI[] _entryUIs;
    [SerializeField] private GameObject _entriesPanel;
    private int _currentPage;
    private HighscoreData _currentDataOpen;

    private void Awake() {
        s_instance = this;
        _regularSingleScores = JSONTool.ReadData<HighscoreData>("_regularSingleScores.json");
        _regularTripleScores = JSONTool.ReadData<HighscoreData>("_regularTripleScores.json");
        _regularPentaScores = JSONTool.ReadData<HighscoreData>("_regularPentaScores.json");
        _challengeSingleScores = JSONTool.ReadData<HighscoreData>("_challengeSingleScores.json");
        _challengeTripleScores = JSONTool.ReadData<HighscoreData>("_challengeTripleScores.json");
        _challengePentaScores = JSONTool.ReadData<HighscoreData>("_challengePentaScores.json");

        _currentPage = 0;
    }

    public void OpenScores() {
        bool regular = !ConfigurationManager.s_instance.ChallengeMode;
        int seriesLength = ConfigurationManager.s_instance.SeriesLength;
        OpenScores(regular, seriesLength);
    }

    public void OpenScores(bool regular, int seriesLength) {
        _currentDataOpen = GetHighscoreData(regular, seriesLength);
        _currentPage = 0;
        _entriesPanel.gameObject.SetActive(true);
        _currentDataOpen.Highscores.Sort((x, y) => y.CompareTo(x));
        DisplayScores(_currentPage);
    }

    public void DisplayScores(int page) {
        int startIndex = page * 10 + 1;
        for (int i = 0; i < 10; i++) {
            if (i + startIndex - 1 >= _currentDataOpen.Highscores.Count) {
                _entryUIs[i].gameObject.SetActive(false);
            } else {
                _entryUIs[i].gameObject.SetActive(true);
                _entryUIs[i].SetEntry(_currentDataOpen.Highscores[i + startIndex - 1], i + startIndex);
                if (startIndex + i == 1) {
                    _entryUIs[i].SetColor(new Color(255f/255f, 229f/255f, 0f/255f, 1f));
                } else if (startIndex + i == 2) {
                    _entryUIs[i].SetColor(new Color(201/255f, 208/255f, 217f/255f, 1f));
                } else if (startIndex + i == 3) {
                    _entryUIs[i].SetColor(new Color(243/255f, 193/255f, 96f/255f, 1f));
                } else {
                    _entryUIs[i].SetColor(Color.white);
                }
            }
        }
    }

    public void RefreshScores() {
        _currentDataOpen = GetHighscoreData(!ConfigurationManager.s_instance.ChallengeMode, ConfigurationManager.s_instance.SeriesLength);
        _currentPage = 0;
        _currentDataOpen.Highscores.Sort((x, y) => y.CompareTo(x));
        DisplayScores(_currentPage);
    }

    public void ChangePage(int change) {
        if (_currentPage + change < 0 || _currentDataOpen == null) {
            return;
        }
        if ((_currentPage + change) * 10 > _currentDataOpen.Highscores.Count) {
            return;
        }

        _currentPage += change;
        DisplayScores(_currentPage);
    }

    public void CloseHighscores() {
        _currentDataOpen = null;
        _entriesPanel.gameObject.SetActive(false);
    }

    private HighscoreData GetHighscoreData(bool regular, int seriesLength) {
        if (regular) {
            switch (seriesLength) {
                case 5:
                    return _regularPentaScores;
                case 3:
                    return _regularTripleScores;
                default:
                    return _regularSingleScores;
            }
        } else {
            switch (seriesLength) {
                case 5:
                    return _challengePentaScores;
                case 3:
                    return _challengeTripleScores;
                default:
                    return _challengeSingleScores;
            }
        }
    }
}
public class HighScoreEntry {
    public string Name;
    public int Score;

    public HighScoreEntry(string name, int score) {
        Name = name;
        Score = score;
    }
}
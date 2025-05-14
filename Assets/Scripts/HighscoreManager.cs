using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;

public class HighscoreManager : MonoBehaviour
{
    public static HighscoreManager s_instance;
    public const int MAX_ENTRIES = 100;
    public const int ENTRIES_PER_PAGE = 5;
    private List<HighscoreData> _highscores;
    [SerializeField] private EntryUI[] _entryUIs;
    [SerializeField] private GameObject _entriesPanel;
    private int _currentPage;
    private HighscoreData _currentDataOpen;
    [SerializeField] private TMP_Text _headerText;

    private void Awake()
    {
        if (s_instance != null)
        {
            Destroy(s_instance);
        }
        s_instance = this;
        _highscores = new List<HighscoreData>();
        for (int i = 0; i < 5; i++)
        {
            string fileName = $"Difficulty{i+1}Scores.json";
            _highscores.Add(JSONTool.ReadData<HighscoreData>(fileName));
        }

        _currentPage = 0;
        LoadScores(1);
    }

    public void LoadScores(int difficultyLevel)
    {
        _headerText.text = "Highscores";
        _currentDataOpen = GetHighscoreData(difficultyLevel);
        _currentPage = 0;
        _entriesPanel.gameObject.SetActive(true);
        _currentDataOpen.Highscores.Sort((x, y) => y.CompareTo(x));
        DisplayScores(_currentPage);
    }

    public void DisplayScores(int page)
    {
        int startIndex = page * ENTRIES_PER_PAGE + 1;
        for (int i = 0; i < ENTRIES_PER_PAGE; i++)
        {
            if (i + startIndex - 1 >= _currentDataOpen.Highscores.Count)
            {
                _entryUIs[i].gameObject.SetActive(false);
            }
            else
            {
                _entryUIs[i].gameObject.SetActive(true);
                _entryUIs[i].SetEntry(_currentDataOpen.Highscores[i + startIndex - 1]);
                if (startIndex + i == 1)
                {
                    _entryUIs[i].SetColor(new Color(255f/255f, 229f/255f, 0f/255f, 1f));
                }
                else if (startIndex + i == 2)
                {
                    _entryUIs[i].SetColor(new Color(201/255f, 208/255f, 217f/255f, 1f));
                }
                else if (startIndex + i == 3)
                {
                    _entryUIs[i].SetColor(new Color(243/255f, 193/255f, 96f/255f, 1f));
                }
                else
                {
                    _entryUIs[i].SetColor(Color.white);
                }
            }
        }
    }

    public void ChangePage(int change)
    {
        if (_currentPage + change < 0 || _currentDataOpen == null)
        {
            return;
        }
        if ((_currentPage + change) * ENTRIES_PER_PAGE > _currentDataOpen.Highscores.Count)
        {
            return;
        }

        _currentPage += change;
        DisplayScores(_currentPage);
    }

    public void CloseHighscores() {
        _currentDataOpen = null;
        _entriesPanel.gameObject.SetActive(false);
    }

    private HighscoreData GetHighscoreData(int difficultyLevel)
    {
        return _highscores[difficultyLevel - 1];
    }

    public void ClearHighscores()
    {
        for (int i = 0; i < 5; i++)
        {
            _highscores[i].Highscores.Clear();
            JSONTool.WriteData(_highscores[i], $"Difficulty{i+1}Scores.json");
        }
        DisplayScores(0);
    }
}
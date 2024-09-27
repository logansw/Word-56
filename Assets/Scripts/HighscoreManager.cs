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
    private HighscoreData _dailyScores;
    private HighscoreData _endlessScores;
    [SerializeField] private EntryUI[] _entryUIs;
    [SerializeField] private GameObject _entriesPanel;
    private int _currentPage;
    private HighscoreData _currentDataOpen;

    private void Awake()
    {
        s_instance = this;
        _dailyScores = JSONTool.ReadData<HighscoreData>("_dailyScores.json");
        _endlessScores = JSONTool.ReadData<HighscoreData>("_endlessScores.json");

        _currentPage = 0;
    }

    public void OpenScores(bool daily)
    {
        _currentDataOpen = GetHighscoreData(daily);
        _currentPage = 0;
        _entriesPanel.gameObject.SetActive(true);
        _currentDataOpen.Highscores.Sort((x, y) => y.CompareTo(x));
        DisplayScores(_currentPage);
    }

    public void DisplayScores(int page)
    {
        int startIndex = page * 10 + 1;
        for (int i = 0; i < 5; i++)
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
        if ((_currentPage + change) * 10 > _currentDataOpen.Highscores.Count)
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

    private HighscoreData GetHighscoreData(bool daily)
    {
        if (daily)
        {
            return _dailyScores;
        }
        else
        {
            return _endlessScores;
        }
    }
}
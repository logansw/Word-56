using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurationManager : MonoBehaviour
{
    // Static
    public static ConfigurationManager s_instance;

    // Public
    public bool ChallengeMode;
    public int SeriesLength;
    public bool ConsecutiveVowelsEnabled;
    public int StartingScore = 30_000;
    public int InflationRateOne = 25;
    public int InflationRateTwo = 50;
    public int InflationRateThree = 75;
    public int PeriodOneStart = 3;
    public int PeriodTwoStart = 9;
    public int PeriodThreeStart = 15;
    public int InflationRateVowel = 25;
    public int InflationRateSolve = 100;
    public int SolveStartCost = 700;

    // External References
    [SerializeField] private UnityEngine.UI.Image[] _singleButtons;
    [SerializeField] private UnityEngine.UI.Image[] _tripleButtons;
    [SerializeField] private UnityEngine.UI.Image[] _pentaButtons;
    [SerializeField] private UnityEngine.UI.Image[] _regularButtons;
    [SerializeField] private UnityEngine.UI.Image[] _challengeButtons;

    void Awake() {
        s_instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        ChallengeMode = false;
        SeriesLength = 1;
    }

    public void SetChallengeMode(bool enabled) {
        ChallengeMode = enabled;
        foreach (UnityEngine.UI.Image button in _regularButtons) {
            button.color = Color.white;
        }
        foreach (UnityEngine.UI.Image button in _challengeButtons) {
            button.color = Color.white;
        }
        UnityEngine.UI.Image[] buttons = enabled ? _challengeButtons : _regularButtons;
        foreach (UnityEngine.UI.Image button in buttons) {
            button.color = new Color(255/255f, 95/255f, 212/255f, 1f);
        }
    }

    public void SetSeriesLength(int numberOfRounds) {
        SeriesLength = numberOfRounds;
        foreach (UnityEngine.UI.Image button in _singleButtons) {
            button.color = Color.white;
        }
        foreach (UnityEngine.UI.Image button in _tripleButtons) {
            button.color = Color.white;
        }
        foreach (UnityEngine.UI.Image button in _pentaButtons) {
            button.color = Color.white;
        }
        UnityEngine.UI.Image[] buttons;
        switch (numberOfRounds) {
            case 1:
                buttons = _singleButtons;
                break;
            case 3:
                buttons = _tripleButtons;
                break;
            case 5:
                buttons = _pentaButtons;
                break;
            default:
                buttons = _singleButtons;
                break;
        }
        foreach (UnityEngine.UI.Image button in buttons) {
            button.color = new Color(255/255f, 95/255f, 212/255f, 1f);
        }
    }
}

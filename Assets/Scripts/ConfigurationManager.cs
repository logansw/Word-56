using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurationManager : MonoBehaviour
{
    // Static
    public static ConfigurationManager s_instance;

    // Public
    public bool ChallengeMode;
    public int SeriesLength;
    public bool ConsecutiveVowelsAllowed;
    public int StartingScore;
    public int InflationRateOne;
    public int InflationRateTwo;
    public int InflationRateThree;
    public int PeriodOneStart;
    public int PeriodTwoStart;
    public int PeriodThreeStart;
    public int InflationRateVowel;
    public int InflationRateSolve;
    public int SolveStartCost;

    // External References
    [SerializeField] private UnityEngine.UI.Image[] _singleButtons;
    [SerializeField] private UnityEngine.UI.Image[] _tripleButtons;
    [SerializeField] private UnityEngine.UI.Image[] _pentaButtons;
    [SerializeField] private UnityEngine.UI.Image[] _regularButtons;
    [SerializeField] private UnityEngine.UI.Image[] _challengeButtons;

    void Awake() {
        if (s_instance != null) {
            Destroy(s_instance.gameObject);
        }
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

    public int GetVowelInflationCost()
    {
        if (WordManager.s_instance.VowelsPurchased == 0)
        {
            return 50;
        }
        else if (WordManager.s_instance.VowelsPurchased == 1)
        {
            return 75;
        }
        else
        {
            return 125;
        }
    }

    public int GetCommonInflationCost(int commonsPurchased)
    {
        if (commonsPurchased == 0)
        {
            return 50;
        }
        else if (commonsPurchased == 1)
        {
            return 75;
        }
        else if (commonsPurchased >= 2)
        {
            return 125;
        }
        else
        {
            return 0;
        }
    }

    public int GetConsonantInflationCost(int lettersPurchased, char letter)
    {
        if (lettersPurchased == 6 && WordManager.s_instance.RowTwoLetters.Contains(letter))
        {
            return 50;
        }
        else if (lettersPurchased == 10)
        {
            if (WordManager.s_instance.RowTwoLetters.Contains(letter))
            {
                return 50;
            }
            else if (WordManager.s_instance.RowThreeLetters.Contains(letter))
            {
                return 25;
            }
            else
            {
                return 0;
            }
        }
        else if (!ChallengeMode && lettersPurchased == 15)
        {
            return 25;
        }
        else
        {
            return 0;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using TMPro;
using System.Text;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Linq;

public class WordManager : MonoBehaviour {
    // Static
    public static WordManager s_instance;
    public List<string> FiveLetterWords;
    public List<string> SixLetterWords;
    public string WordA;
    public string WordB;
    private bool _initialized;
    public HashSet<Letter> LettersFound;

    public int LettersPurchased;
    public int VowelsPurchased;
    public bool VowelPurchasedLastRound;
    public int CommonsPurchased;
    public readonly char[] CommonLetters = {'R', 'S', 'T', 'L'};
    public readonly char[] RowOneLetters = {'R', 'T', 'N', 'S', 'L', 'G', 'D', 'P'};
    public readonly char[] RowTwoLetters = {'M', 'H', 'C', 'B', 'F', 'Y', 'W'};
    public readonly char[] RowThreeLetters = {'K', 'V', 'X', 'J', 'Q', 'Z'};
     // External References
    [SerializeField] private TMP_Text _guessCounterText;
    [SerializeField] private TMP_Text _solveCostText;
    [SerializeField] private TMP_Text _currentScoreText;
    public List<Letter> Letters;
    public int RoundScore;
    [SerializeField] private Button _enterButton;
    private DateTime _startTime;
    public int LetterPurchases;
    public int TimeElapsed;
    public int SolvePurchases;
    public int SelectionBonus;
    public int TimeBonus;
    [SerializeField] private ScoreBreakdown _scoreBreakdownPrefab;
    [SerializeField] private Canvas _intermisisonCanvas;
    public int SolvePurchaseCost;
    private ConfigurationManager _configMan;
    public List<SolveLetter> SolveLetters = new List<SolveLetter>();
    public char SelectedLetter;

    void Awake() {
        s_instance = this;
    }

    void Start() {
        _configMan = ConfigurationManager.s_instance;
    }

    public void StartRound() {
        StateController.s_instance.ChangeState(StateController.s_instance.BuyState);
        _guessCounterText.text = "Guesses: 0";
        RoundScore = _configMan.StartingScore;
        _startTime = DateTime.Now;
        RenderCurrentScore();
    }

    void OnEnable() {
        Letter.e_OnLetterClicked += OnLetterClicked;
    }

    void OnDisable() {
        Letter.e_OnLetterClicked -= OnLetterClicked;
    }

    public void Initialize() {
        if (_initialized) { return; }
        FiveLetterWords = new List<string>();
        string filepathFive = Application.streamingAssetsPath + "/FiveLetterWords.txt";
        try {
            using (StreamReader reader = new StreamReader(filepathFive)) {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    string[] words = line.Split('\t');
                    FiveLetterWords.AddRange(words);
                }
            }
        } catch (Exception e) {
            Debug.Log(e.Message);
        }

        SixLetterWords = new List<string>();
        string filepathSix = Application.streamingAssetsPath + "/SixLetterWords.txt";
        try {
            using (StreamReader reader = new StreamReader(filepathSix)) {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    string[] words = line.Split('\t');
                    SixLetterWords.AddRange(words);
                }
            }
        } catch (Exception e) {
            Debug.Log(e.Message);
        }

        SetLetterStates();

        _initialized = true;
    }

    public void Reset()
    {
        _configMan ??= ConfigurationManager.s_instance;
        LettersFound = new HashSet<Letter>();
        foreach (SolveLetter solveLetter in SolveLetters) {
            solveLetter.Clear();
        }
        foreach (Letter letter in Letters)
        {
            letter.Reset();
        }
        LettersPurchased = 0;
        VowelsPurchased = 0;
        CommonsPurchased = 0;
        LetterPurchases = 0;
        TimeElapsed = 0;
        SolvePurchases = 0;
        _guessCounterText.text = "Guesses: 0";
        SolvePurchaseCost = _configMan.SolveStartCost;
        _solveCostText.text = $"Solve ({SolvePurchaseCost})";
        _currentScoreText.text = "";
        RoundScore = _configMan.StartingScore;
        SetLetterStates();
    }

    public void ChooseWords()
    {
        string wordA = ChooseRandomWord(FiveLetterWords);
        string wordB = ChooseRandomWord(SixLetterWords);
        if (wordA.Trim().Length != 5 || wordB.Trim().Length != 6)
        {
            Debug.Log("Invalid word lengths.");
        }
        WordA = wordA;
        WordB = wordB;
    }

    private string ChooseRandomWord(List<string> words)
    {
        int index = UnityEngine.Random.Range(0, words.Count);
        return words[index];
    }

    private int CountVowels(string word)
    {
        int count = 0;
        foreach (char c in word)
        {
            if (c == 'A' || c == 'E' || c == 'I' || c == 'O' || c == 'U')
            {
                count++;
            }
        }
        return count;
    }

    private void OnLetterClicked(Letter letter)
    {
        if (StateController.GetCurrentState() == State.StateType.Buy)
        {
            PurchaseLetter(letter);
            HandleGuess(letter);
            _guessCounterText.text = "Guesses: " + LettersPurchased;
            SetLetterStates();
            ChallengeModeSetCosts();
        }
        else if (StateController.GetCurrentState() == State.StateType.Solve)
        {
            RenderLetters();
            SelectLetter(letter.Character);
            letter.ColorLetterForSelection();
        }
    }

    private void SelectLetter(char c)
    {
        SelectedLetter = c;
    }

    public string GetCurrentGuess()
    {
        string guess = "";
        foreach (SolveLetter solveLetter in SolveLetters)
        {
            if (solveLetter.Character != '\0')
            {
                guess += solveLetter.Character;
            }
        }
        return guess;
    }

    public void CancelSolveAttempt()
    {
        foreach (SolveLetter solveLetter in SolveLetters)
        {
            if (solveLetter.Status == SolveStatus.Guess)
            {
                solveLetter.Clear();
            }
        }
        RenderLetters();
        SelectedLetter = '\0';

    }

    public void SubmitSolveAttempt() {
        SolvePurchases += SolvePurchaseCost;
        RoundScore -= SolvePurchaseCost;
        RenderCurrentScore();
        // Check if the solve attempt is correct
        string solveAttempt = GetCurrentGuess();
        if (solveAttempt.Substring(0, 5) == WordA && solveAttempt.Substring(5, 6) == WordB) {
            Solve();
        } else {
            CancelSolveAttempt();
            SolvePurchaseCost += _configMan.InflationRateSolve;
            _solveCostText.text = $"Solve ({SolvePurchaseCost})";
            AudioManager.s_instance.Negative.Play();
        }
    }

    private void Solve()
    {
        TimeSpan timeSpan = DateTime.Now - _startTime;
        int seconds = (int)timeSpan.TotalSeconds;
        TimeElapsed = seconds;
        TimeBonus = 1000 - (TimeElapsed / 30 * 50);
        if (TimeElapsed >= 300)
        {
            TimeBonus = 0;
        }
        if (LettersPurchased <= 14)
        {
            SelectionBonus = (int)(50 * Mathf.Pow(2, 14 - LettersPurchased));
        }
        ScoreBreakdown scoreBreakdown = Instantiate(_scoreBreakdownPrefab, _intermisisonCanvas.transform);
        scoreBreakdown.transform.localPosition = new Vector3(0, 80, 0);
        scoreBreakdown.Initialize(GameManager.s_instance.CurrentRound, LetterPurchases, TimeElapsed, SolvePurchases, TimeBonus, SelectionBonus);
        StateController.s_instance.ChangeState(StateController.s_instance.IntermissionState);
        AudioManager.s_instance.Victory.Play();
    }

    private void HandleGuess(Letter letter) {
        bool found = false;
        for (int i = 0; i < WordA.Length; i++) {
            if (WordA[i] == letter.Character) {
                found = true;
                RevealLetter(letter.Character, i);
            }
        }

        for (int i = 0; i < WordB.Length; i++) {
            if (WordB[i] == letter.Character) {
                found = true;
                RevealLetter(letter.Character, i + WordA.Length);
            }
        }

        if (found) {
            LettersFound.Add(letter);
            AudioManager.s_instance.Positive.Play();
        } else {
            AudioManager.s_instance.Click.Play();
        }
    }

    private void RevealLetter(char c, int characterIndex) {
        SolveLetters[characterIndex].SetCorrect(c);
    }

    private void PurchaseLetter(Letter letter) {
        LetterPurchases += letter.Cost;
        LettersPurchased++;
        letter.Purchased = true;
        RoundScore -= letter.Cost;
        if (letter.IsVowel()) {
            IncreaseVowelCosts();
            VowelsPurchased++;
            VowelPurchasedLastRound = true;
        } else {
            VowelPurchasedLastRound = false;
            if (CommonLetters.Contains(letter.Character))
            {
                IncreaseCommonCosts();
                CommonsPurchased++;
            }
            else
            {
                IncreaseConsonantCosts();
            }
        }
        RenderCurrentScore();
    }

    private void SetLetterStates() {
        foreach (Letter letter in Letters)
        {
            LetterState state = LetterState.Default;
            char c = letter.Character;
            if (letter.Purchased)
            {
                state = s_instance.LettersFound.Contains(letter) ? LetterState.Correct : LetterState.Incorrect;
            }
            else if (letter.IsVowel())
            {
                if (LettersPurchased == 0 || LettersPurchased == 1 || (!_configMan.ConsecutiveVowelsAllowed && VowelPurchasedLastRound))
                {
                    state = LetterState.Disabled;
                }
            }
            else
            {
                if (LettersPurchased == 0 && RowOneLetters.Contains(c))
                {
                    state = LetterState.Disabled;
                }
            }

            letter.LetterState = state;
        }
        RenderLetters();
    }

    private void RenderLetters() {
        if (LettersPurchased == 1 || LettersPurchased == 2) {
            foreach (Letter letter in Letters) {
                letter.SetCostsToFaceValue();
            }
        }
        foreach (Letter letter in Letters) {
            letter.RenderLetter();
        }
    }

    public void RenderEnterButton() {
        _enterButton.interactable = GetCurrentGuess().Length == 11;
    }

    private void IncreaseVowelCosts() {
        foreach (Letter letter in Letters) {
            if (letter.IsVowel()) {
                letter.IncreaseCost(_configMan.GetVowelInflationCost());
            }
        }
    }

    private void IncreaseCommonCosts() {
        foreach (Letter letter in Letters) {
            if (CommonLetters.Contains(letter.Character))
            {
                letter.IncreaseCost(_configMan.GetCommonInflationCost(CommonsPurchased));
            }
        }
    }

    private void IncreaseConsonantCosts() {
        foreach (Letter letter in Letters) {
            if (!letter.IsVowel())
            {
                letter.IncreaseCost(_configMan.GetConsonantInflationCost(LettersPurchased, letter.Character));
            }
        }
    }

    private void RenderCurrentScore() {
        _currentScoreText.text = "Score: " + RoundScore;
    }

    private void ChallengeModeSetCosts()
    {
        if (!_configMan.ChallengeMode) { return; }
        if (LettersPurchased == 14)
        {
            foreach (Letter letter in Letters)
            {
                if (RowOneLetters.Contains(letter.Character) || letter.IsVowel())
                {
                    letter.SetCost(1200);
                }
                else if (RowTwoLetters.Contains(letter.Character))
                {
                    letter.SetCost(800);
                }
                else if (RowThreeLetters.Contains(letter.Character))
                {
                    letter.SetCost(600);
                }
            }
        }
    }
}
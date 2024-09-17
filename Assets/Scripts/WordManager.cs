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

     // External References
    [SerializeField] private TMP_Text _guessCounterText;
    [SerializeField] private TMP_Text _solveCounterText;
    public List<Letter> Letters;
    [SerializeField] private Button _enterButton;
    private DateTime _startTime;
    public int LetterPurchases;
    public int TimeElapsed;
    public int SolvePurchases;
    public int SelectionBonus;
    public int TimeBonus;
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
        _guessCounterText.text = "Guesses Left: 13";
        _solveCounterText.text = "Solves Left: 2";
        _startTime = DateTime.Now;
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
        _guessCounterText.text = "Guesses Left: 13";
        _solveCounterText.text = "Solves Left: 2";
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
            _guessCounterText.text = "Guesses: " + (13 - LettersPurchased);
            SetLetterStates();
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
        SolvePurchases++;
        // Check if the solve attempt is correct
        string solveAttempt = GetCurrentGuess();
        if (solveAttempt.Substring(0, 5) == WordA && solveAttempt.Substring(5, 6) == WordB) {
            Solve();
        } else {
            CancelSolveAttempt();
            AudioManager.s_instance.Negative.Play();
            if (SolvePurchases >= 2)
            {
                StateController.s_instance.ChangeState(StateController.s_instance.DefeatState);
            }
        }
        _solveCounterText.text = "Solves: " + (2 - SolvePurchases);
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
        LettersPurchased++;
        letter.Purchased = true;
        if (letter.IsVowel()) {
            VowelsPurchased++;
            VowelPurchasedLastRound = true;
        } else {
            VowelPurchasedLastRound = false;
        }
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

            letter.LetterState = state;
        }

        if (LettersPurchased >= 13)
        {
            foreach (Letter letter in Letters)
            {
                if (!letter.Purchased)
                {
                    letter.LetterState = LetterState.Disabled;
                }
            }
            RenderLetters();
        }

        RenderLetters();
    }

    private void RenderLetters() {
        foreach (Letter letter in Letters) {
            letter.RenderLetter();
        }
    }

    public void RenderEnterButton() {
        _enterButton.interactable = GetCurrentGuess().Length == 11;
    }
}
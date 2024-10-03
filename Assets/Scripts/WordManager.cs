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
using Unity.VisualScripting.FullSerializer;

public class WordManager : MonoBehaviour {
    // Static
    public static WordManager s_instance;
    public List<string> FiveLetterWords;
    public List<string> SixLetterWords;
    public string WordA;
    public string WordB;
    private bool _initialized;
    public HashSet<Letter> LettersFound;

    private int _lettersRemaining;
    public int LettersRemaining
    {
        get
        {
            return _lettersRemaining;
        }
        set
        {
            _lettersRemaining = value;
            _guessCounterText.text = "Guesses Left: " + LettersRemaining;
        }
    }
    private int _solvesRemaining;
    public int SolvesRemaining
    {
        get
        {
            return _solvesRemaining;
        }
        set
        {
            _solvesRemaining = value;
            _solveCounterText.text = "Solves Left: " + SolvesRemaining;
        }
    }

    public bool VowelPurchasedLastRound;

     // External References
    [SerializeField] private TMP_Text _guessCounterText;
    [SerializeField] private TMP_Text _solveCounterText;
    public List<Letter> Letters;
    [SerializeField] private Button _enterButton;
    [SerializeField] private Canvas _intermisisonCanvas;
    private ConfigurationManager _configMan;
    public List<SolveLetter> SolveLetters = new List<SolveLetter>();
    public char SelectedLetter;
    public bool IsSecondChance;
    public bool Victory;
    public TMP_Text FinalAnswerText;


    void Awake() {
        s_instance = this;
        _configMan = ConfigurationManager.s_instance;
    }

    public void StartRound() {
        StateController.s_instance.ChangeState(StateController.s_instance.BuyState);
    }

    void OnEnable() {
        Letter.e_OnLetterClicked += OnLetterClicked;
    }

    void OnDisable() {
        Letter.e_OnLetterClicked -= OnLetterClicked;
    }

    public void Initialize()
    {
        if (_initialized) { return; }
        FiveLetterWords = new List<string>();
        string filepathFive = Application.streamingAssetsPath + "/FiveLetterWords.txt";
        try
        {
            using (StreamReader reader = new StreamReader(filepathFive))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] words = line.Split('\t');
                    FiveLetterWords.AddRange(words);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        SixLetterWords = new List<string>();
        string filepathSix = Application.streamingAssetsPath + "/SixLetterWords.txt";
        try
        {
            using (StreamReader reader = new StreamReader(filepathSix)) {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    string[] words = line.Split('\t');
                    SixLetterWords.AddRange(words);
                }
            }
        }
        catch (Exception e)
        {
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
        LettersRemaining = 13;
        SolvesRemaining = 2;
        SetLetterStates();
        Victory = false;
    }

    public void ChooseWords()
    {
        string wordA = ChooseRandomWord(FiveLetterWords, ConfigurationManager.s_instance.IsDailyMode);
        string wordB = ChooseRandomWord(SixLetterWords, ConfigurationManager.s_instance.IsDailyMode);
        if (wordA.Trim().Length != 5 || wordB.Trim().Length != 6)
        {
            Debug.Log("Invalid word lengths.");
        }
        WordA = wordA;
        WordB = wordB;
        FinalAnswerText.text = WordA + "\n" + WordB;
    }

    private string ChooseRandomWord(List<string> words, bool isDailyMode)
    {
        if (isDailyMode)
        {
            int y = System.DateTime.Now.Year;
            int m = System.DateTime.Now.Month;
            int d = System.DateTime.Now.Day;
            System.DateTime currentDateTime = new System.DateTime(y, m, d);
            int seed = currentDateTime.GetHashCode();
            seed = seed < 0 ? -seed : seed;
            System.Random random = new System.Random(seed);
            int index = random.Next(0, words.Count);
            return words[index];
        }
        else
        {
            int index = UnityEngine.Random.Range(0, words.Count);
            return words[index];
        }
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
        if (StateController.GetCurrentState() == State.StateType.Buy && LettersRemaining > 0)
        {
            PurchaseLetter(letter);
            HandleGuess(letter);
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
        SolvesRemaining--;
        // Check if the solve attempt is correct
        string solveAttempt = GetCurrentGuess();
        if (solveAttempt.Substring(0, 5) == WordA && solveAttempt.Substring(5, 6) == WordB) {
            Solve();
        } else {
            CancelSolveAttempt();
            AudioManager.s_instance.Negative.Play();
            if (SolvesRemaining <= 0)
            {
                StateController.s_instance.ChangeState(StateController.s_instance.DefeatState);
                HighscoreWriter.s_Instance.SetOutcomeText(false, IsSecondChance);
            }
        }
    }

    private void Solve()
    {
        Victory = true;
        HighscoreWriter.s_Instance.CheckOnLeaderboard();
        AudioManager.s_instance.Victory.Play();
        Timer.s_instance.StopTimer();
        HighscoreWriter.s_Instance.SetOutcomeText(true, IsSecondChance);
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
        LettersRemaining--;
        letter.Purchased = true;
        if (letter.IsVowel()) {
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
                if (!IsSecondChance && (LettersRemaining == 13 || LettersRemaining == 12 || (!_configMan.ConsecutiveVowelsAllowed && VowelPurchasedLastRound)))
                {
                    state = LetterState.Disabled;
                }
            }

            letter.LetterState = state;
        }

        if (LettersRemaining == 0)
        {
            foreach (Letter letter in Letters)
            {
                if (letter.LetterState == LetterState.Default)
                {
                    if (StateController.GetCurrentState() == State.StateType.Buy)
                    {
                        letter.LetterState = LetterState.Disabled;
                    }
                }
            }
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

    public void GiveSecondChance()
    {
        if (IsSecondChance)
        {
            return;
        }
        IsSecondChance = true;
        StateController.s_instance.ChangeState(StateController.s_instance.BuyState);
        GameManager.s_instance.SetActivePanel(0);
        LettersRemaining += 2;
        SolvesRemaining += 1;
        Timer.s_instance.StartTimer(30);
    }
}
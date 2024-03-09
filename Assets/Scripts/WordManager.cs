using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using TMPro;
using System.Text;
using UnityEngine.UI;

public class WordManager : MonoBehaviour {
    // Static
    public static WordManager s_instance;
    public List<string> FiveLetterWords;
    public List<string> SixLetterWords;
    public string WordA;
    public string WordB;
    public TMP_Text WordAText;
    public TMP_Text WordBText;
    private bool _initialized;
    public HashSet<Letter> LettersFound;

    public int LettersPurchased;
    public int VowelsPurchased;
    public bool VowelPurchasedLastRound;
     // External References
    [SerializeField] private TMP_Text _guessCounterText;
    [SerializeField] private TMP_Text _solveAttemptText;
    private string _solveAttempt;
    public List<Letter> Letters;
    public int RoundScore;
    private int _solveIndex;
    [SerializeField] private Button _enterButton;
    private DateTime _startTime;
    public int LetterPurchases;
    public int TimePenalty;
    public int SolvePurchases;
    [SerializeField] private ScoreBreakdown _scoreBreakdownPrefab;
    [SerializeField] private Canvas _intermisisonCanvas;

    void Awake() {
        s_instance = this;
    }

    public void StartRound() {
        StateController.s_instance.ChangeState(StateController.s_instance.BuyState);
        _guessCounterText.text = "Guesses: 0";
        RoundScore = 30000;
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
        string filepathFive = Application.dataPath + "/Resources/FiveLetterWords.txt";
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
        string filepathSix = Application.dataPath + "/Resources/SixLetterWords.txt";
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

        _initialized = true;
    }

    public void Reset() {
        LettersFound = new HashSet<Letter>();
        WordAText.text = "_ _ _ _ _";
        WordBText.text = "_ _ _ _ _ _";
        _solveAttempt = "";
        _solveAttemptText.text = "";
        foreach (Letter letter in Letters) {
            letter.Reset();
        }
        LettersPurchased = 0;
        VowelsPurchased = 0;
        LetterPurchases = 0;
        TimePenalty = 0;
        SolvePurchases = 0;
        _solveIndex = 0;
        _guessCounterText.text = "Guesses: 0";
    }

    public void ChooseWords(int vowelCount) {
        int a; int b = 0;
        string wordA = ChooseRandomWord(FiveLetterWords);
        string wordB = "";
        a = CountVowels(wordA);
        while (a == vowelCount) {
            wordA = ChooseRandomWord(FiveLetterWords);
            a = CountVowels(wordA);
        }
        while (a + b != vowelCount) {
            wordB = ChooseRandomWord(SixLetterWords);
            b = CountVowels(wordB);
        }
        if (wordA.Trim().Length != 5 || wordB.Trim().Length != 6) {
            Debug.Log("Invalid word lengths.");
        }
        WordA = wordA;
        WordB = wordB;
    }

    private string ChooseRandomWord(List<string> words) {
        int index = UnityEngine.Random.Range(0, words.Count);
        return words[index];
    }

    private int CountVowels(string word) {
        int count = 0;
        foreach (char c in word) {
            if (c == 'A' || c == 'E' || c == 'I' || c == 'O' || c == 'U') {
                count++;
            }
        }
        return count;
    }

    private void OnLetterClicked(Letter letter) {
        if (StateController.GetCurrentState() == State.StateType.Buy) {
            PurchaseLetter(letter);
            HandleGuess(letter);
            _guessCounterText.text = "Guesses: " + LettersPurchased;
            SetLetterStates();
            RenderLetters();
        } else if (StateController.GetCurrentState() == State.StateType.Solve) {
            AddLetterToSolveAttempt(letter);
        }
    }

    private void AddLetterToSolveAttempt(Letter letter) {
        _solveAttempt += letter.Character;
        _solveIndex++;
        _solveAttemptText.text = _solveAttempt;
        _enterButton.interactable = _solveAttempt.Length == 11;
    }

    public void DeleteLetterFromSolveAttempt() {
        if (_solveIndex == 0) { return; }
        _solveIndex--;
        _solveAttempt = _solveAttempt.Substring(0, _solveIndex);
        _solveAttemptText.text = _solveAttempt;
        _enterButton.interactable = _solveAttempt.Length == 11;
    }

    public void SubmitSolveAttempt() {
        SolvePurchases += 1000;
        if (_solveAttempt.Substring(0, 5) == WordA && _solveAttempt.Substring(5, 6) == WordB) {
            TimeSpan timeSpan = DateTime.Now - _startTime;
            int seconds = (int)timeSpan.TotalSeconds;
            TimePenalty = seconds * 10;
            ScoreBreakdown scoreBreakdown = Instantiate(_scoreBreakdownPrefab, _intermisisonCanvas.transform);
            scoreBreakdown.transform.localPosition = new Vector3(0, 80, 0);
            scoreBreakdown.Initialize(GameManager.s_instance.CurrentRound, LetterPurchases, TimePenalty, SolvePurchases);
            if (ConfigurationManager.s_instance.SeriesLength == 1) {
                GameManager.s_instance.LetterPurchases.Add(LetterPurchases);
                GameManager.s_instance.TimePenalties.Add(TimePenalty);
                GameManager.s_instance.SolvePurchases.Add(SolvePurchases);
                
                StateController.s_instance.ChangeState(StateController.s_instance.IntermissionState);
                StateController.s_instance.ChangeState(StateController.s_instance.GameOverState);
            } else {
                StateController.s_instance.ChangeState(StateController.s_instance.IntermissionState);
            }
        } else {
            _solveAttemptText.text = "";
            _solveAttempt = "";
            _solveIndex = 0;
            StateController.s_instance.ChangeState(StateController.s_instance.BuyState);
        }
    }

    private void HandleGuess(Letter letter) {
        bool found = false;
        for (int i = 0; i < WordA.Length; i++) {
            if (WordA[i] == letter.Character) {
                found = true;
                RevealLetter(0, i);
            }
        }

        for (int i = 0; i < WordB.Length; i++) {
            if (WordB[i] == letter.Character) {
                found = true;
                RevealLetter(1, i);
            }
        }

        if (found) {
            LettersFound.Add(letter);
            // Play ding sound
        } else {
            // Play dull sound
            // Inform the player why this letter is not in the word
        }
    }

    private void RevealLetter(int wordIndex, int characterIndex) {
        if (wordIndex == 0) {
            char c = WordA[characterIndex];
            StringBuilder sb = new StringBuilder(WordAText.text);
            sb[characterIndex * 2] = c;
            WordAText.text = sb.ToString();
        } else {
            char c = WordB[characterIndex];
            StringBuilder sb = new StringBuilder(WordBText.text);
            sb[characterIndex * 2] = c;
            WordBText.text = sb.ToString();
        }
    }

    private void PurchaseLetter(Letter letter) {
        LetterPurchases += letter.Cost;
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
        foreach (Letter letter in Letters) {
            LetterState state = LetterState.Default;
            if (letter.Purchased) {
                state = WordManager.s_instance.LettersFound.Contains(letter) ? LetterState.Correct : LetterState.Incorrect;
            } else if (letter.IsVowel()) {
                if (LettersPurchased < 5) {
                    state = LetterState.Disabled;
                }
                if (VowelPurchasedLastRound) {
                    state = LetterState.Disabled;
                }
                if (LettersPurchased < 10 && VowelsPurchased >= 1) {
                    state = LetterState.Disabled;
                }
            }

            letter.LetterState = state;
        }
    }

    private void RenderLetters() {
        foreach (Letter letter in Letters) {
            letter.RenderLetter();
        }
    }
}
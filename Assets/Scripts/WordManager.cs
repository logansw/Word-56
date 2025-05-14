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
using System.Data;
using UnityEngine.U2D;
using Unity.VisualScripting;

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
    private int _vowelsPurchased;
    [SerializeField] private List<EnablerDisabler> _toEnable;
    [SerializeField] private List<EnablerDisabler> _toDisable;
    


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
                    string[] words = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
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
                    string[] words = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                    SixLetterWords.AddRange(words);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        LettersRemaining = _configMan.LetterGuesses;
        SolvesRemaining = _configMan.WordGuesses;
        _vowelsPurchased = 0;
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
        LettersRemaining = _configMan.LetterGuesses;
        SolvesRemaining = _configMan.WordGuesses;
        SetLetterStates();
        Victory = false;
        _vowelsPurchased = 0;
    }

    public void ChooseWords()
    {
        string wordA = ChooseRandomWord(FiveLetterWords, false);
        string wordB = ChooseRandomWord(SixLetterWords, false);
        if (wordA.Trim().Length != 5 || wordB.Trim().Length != 6)
        {
            Debug.Log("Invalid word lengths.");
            if (wordA.Trim().Length != 5)
            {
                Debug.Log("Word A: " + wordA);
            }
            if (wordB.Trim().Length != 6)
            {
                Debug.Log("Word B: " + wordB);
            }
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
        if (solveAttempt.Substring(0, 5) == WordA && solveAttempt.Substring(5, 6) == WordB)
        {
            AudioManager.s_instance.Victory.Play();
            Timer.s_instance.PauseTimer();
            StartCoroutine(AnimateSolve());
        }
        else
        {
            CancelSolveAttempt();
            AudioManager.s_instance.Negative.Play();
            if (SolvesRemaining <= 0)
            {
                if (IsSecondChance)
                {
                    StateController.s_instance.ChangeState(StateController.s_instance.GameOverState);
                }
                else
                {
                    StateController.s_instance.ChangeState(StateController.s_instance.SecondChanceState);
                }
                HighscoreWriter.s_Instance.SetOutcomeText(false, IsSecondChance);
            }
        }
    }

    private IEnumerator AnimateSolve()
    {
        for (int i = 0; i < SolveLetters.Count; i++)
        {
            StartCoroutine(AnimateLetter(SolveLetters[i]));
            yield return new WaitForSeconds(0.12f);
        }
        yield return new WaitForSeconds(0.5f);
        Solve();
    }

    private IEnumerator AnimateLetter(SolveLetter solveLetter)
    {
        float timeDelta = 0;
        float duration = 0.25f;
        Vector2 originalPosition = solveLetter.transform.position;
        while (timeDelta < duration)
        {
            timeDelta += Time.deltaTime;
            float t = timeDelta / duration;
            if (t < 0.5f)
            {
                solveLetter.transform.localPosition += new Vector3(0f, Time.deltaTime * 2f, 0f);
            }
            else if (t >= 0.5f)
            {
                solveLetter.SetSolved();
                solveLetter.transform.localPosition -= new Vector3(0f, Time.deltaTime * 2f, 0f);
            }
            yield return null;
        }
        solveLetter.transform.position = originalPosition;
    }

    private void Solve()
    {
        Victory = true;
        HighscoreWriter.s_Instance.SetOutcomeText(true, IsSecondChance);
        StateController.s_instance.ChangeState(StateController.s_instance.GameOverState);
        for (int i = 0; i < WordA.Length; i++)
        {
            char c = WordA[i];
            RevealLetter(c, i);
        }
        for (int i = 0; i < WordB.Length; i++)
        {
            char c = WordB[i];
            RevealLetter(c, 5+i);
        }
        foreach (SolveLetter solveLetter in SolveLetters)
        {
            solveLetter.SetSolved();
        }
    }

    public void FreezeGame()
    {
        foreach (EnablerDisabler enablerDisabler in _toEnable)
        {
            enablerDisabler.SetButtonEnabled(true);
        }
        foreach (EnablerDisabler enablerDisabler in _toDisable)
        {
            enablerDisabler.SetButtonEnabled(false);
        }
        
        for (int i = 0; i < SolveLetters.Count; i++)
        {
            SolveLetter solveLetter = SolveLetters[i];

            if (solveLetter.Status == SolveStatus.Correct)
            {
                solveLetter.SetSolved();
            }
            else if (solveLetter.Status == SolveStatus.Blank)
            {
                if (i < WordA.Length)
                {
                    solveLetter.SetGuess(WordA[i]);
                }
                else
                {
                    solveLetter.SetGuess(WordB[i - WordA.Length]);
                }
            }
        }
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
            _vowelsPurchased++;
        } else {
            VowelPurchasedLastRound = false;
        }
    }

    private void SetLetterStates() {
        int lettersGuessed = _configMan.LetterGuesses - LettersRemaining;
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
                if (_vowelsPurchased >= _configMan.VowelGuesses)
                {
                    state = LetterState.Disabled;
                }
                else if (IsSecondChance && (lettersGuessed + 2 < 3 || (!_configMan.ConsecutiveVowelsAllowed && VowelPurchasedLastRound)))
                {
                    state = LetterState.Disabled;
                }
                else if (!IsSecondChance && (lettersGuessed < 3 || (!_configMan.ConsecutiveVowelsAllowed && VowelPurchasedLastRound)))
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

    public void RenderLetters() {
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
        int lettersGuessed = _configMan.LetterGuesses - LettersRemaining;
        IsSecondChance = true;
        StateController.s_instance.ChangeState(StateController.s_instance.BuyState);
        GameManager.s_instance.SetActivePanel(0);
        LettersRemaining += 2;
        SolvesRemaining += 1;
        Timer.s_instance.StartTimer(40);
        foreach (Letter letter in Letters)
        {
            if (letter.LetterState == LetterState.Disabled)
            {
                letter.LetterState = LetterState.Default;
            }

            if (letter.IsVowel())
            {
                if (_vowelsPurchased >= _configMan.VowelGuesses)
                {
                    letter.LetterState = LetterState.Disabled;
                }
                else if (lettersGuessed + 2 < 3 || (!_configMan.ConsecutiveVowelsAllowed && VowelPurchasedLastRound))
                {
                    letter.LetterState = LetterState.Disabled;
                }
            }
        }
        RenderLetters();
    }
}
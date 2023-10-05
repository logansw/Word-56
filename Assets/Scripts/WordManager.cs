using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using TMPro;
using System.Text;

public class WordManager : MonoBehaviour {
    public List<string> FiveLetterWords;
    public List<string> SixLetterWords;
    public string WordA;
    public string WordB;
    public TMP_Text WordAText;
    public TMP_Text WordBText;
    private bool _initialized;

    void OnEnable() {
        Letter.e_OnLetterClicked += OnLetterClicked;
    }

    void OnDisable() {
        Letter.e_OnLetterClicked -= OnLetterClicked;
    }

    public void InitializeWords() {
        if (_initialized) { return; }
        WordAText.text = "_ _ _ _ _";
        WordBText.text = "_ _ _ _ _ _";
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

    public void ChooseWords(int vowelCount) {
        int a; int b = 0;
        string wordA = ChooseRandomWord(FiveLetterWords);
        string wordB = "";
        a = CountVowels(wordA);
        while (a + b != vowelCount) {
            wordB = ChooseRandomWord(SixLetterWords);
            b = CountVowels(wordB);
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
            switch (letter.LetterState) {
                case LetterState.Default:
                    HandleGuess(letter);
                    break;
                case LetterState.Correct:
                    // Play dull sound
                    // Inform the player why this letter cannot be pressed
                    break;
                case LetterState.Disabled:
                    // Play dull sound
                    // Inform the player why this letter is disabled
                    break;
            }
        } else if (StateController.GetCurrentState() == State.StateType.Solve) {

        }
    }

    private void HandleGuess(Letter letter) {
        Debug.Log("HandleGuess");
        GameManager.s_instance.Score -= letter.Cost;
        GameManager.s_instance.LettersPurchased++;
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
            letter.LetterState = LetterState.Correct;
            // Play correct sound
        } else {
            letter.LetterState = LetterState.Disabled;
            // Play incorrect sound
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
}
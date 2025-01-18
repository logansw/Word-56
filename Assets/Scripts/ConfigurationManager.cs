using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurationManager : MonoBehaviour
{
    // Static
    public static ConfigurationManager s_instance;

    // Public
    public bool ConsecutiveVowelsAllowed;

    // External References

    public int CurrentLevel;
    public int SolveTime;
    public int LetterGuesses;
    public int WordGuesses;
    public int VowelGuesses;
    [SerializeField] private Button _increaseDifficultyButton;
    [SerializeField] private Button _decreaseDifficultyButton;
    [SerializeField] private TMP_Text _difficultyText;

    void Awake()
    {
        if (s_instance != null)
        {
            Destroy(s_instance.gameObject);
        }
        s_instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        CurrentLevel = PlayerPrefs.GetInt("DifficultyLevel", 1);
        SetDifficultyLevel(CurrentLevel);
    }

    public void SetDifficultyLevel(int difficultyLevel)
    {
        switch (difficultyLevel)
        {
            case 1:
                SolveTime = 120;
                LetterGuesses = 13;
                WordGuesses = 3;
                VowelGuesses = 5;
                break;
            case 2:
                SolveTime = 105;
                LetterGuesses = 13;
                WordGuesses = 3;
                VowelGuesses = 5;
                break;
            case 3:
                SolveTime = 105;
                LetterGuesses = 12;
                WordGuesses = 3;
                VowelGuesses = 5;
                break;
            case 4:
                SolveTime = 90;
                LetterGuesses = 12;
                WordGuesses = 3;
                VowelGuesses = 4;
                break;
            case 5:
                SolveTime = 90;
                LetterGuesses = 12;
                WordGuesses = 3;
                VowelGuesses = 3;
                break;
        }
        SetDifficultyDescription();
        PlayerPrefs.SetInt("DifficultyLevel", difficultyLevel);
    }

    private void SetDifficultyDescription()
    {
        _difficultyText.text = $"Solve Time: {SolveTime} seconds\n" +
                              $"Letter Guesses: {LetterGuesses}\n" +
                              $"Word Guesses: {WordGuesses}\n" +
                              $"Vowel Guesses: {VowelGuesses}";
    }
}
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
    public DateTime NextDay;

    // External References
    public bool IsDailyMode;
    public bool AlreadyAttempted;
    [SerializeField] private Button _dailyButton;

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

    void Update()
    {
        CheckReset();
    }

    void Initialize()
    {
        CheckReset();
        AlreadyAttempted = PlayerPrefs.GetInt("TodaySolved") == 1;
        _dailyButton.interactable = !AlreadyAttempted;
        NextDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
        PlayerPrefs.SetInt("LastLogInYear", DateTime.Now.Year);
        PlayerPrefs.SetInt("LastLogInMonth", DateTime.Now.Month);
        PlayerPrefs.SetInt("LastLogInDay", DateTime.Now.Day);
        CurrentLevel = 1;
        SetDifficultyLevel(1);
    }

    public void CheckReset()
    {
        if (DateTime.Now.Year > PlayerPrefs.GetInt("LastLogInYear") ||
            DateTime.Now.Month > PlayerPrefs.GetInt("LastLogInMonth") ||
            DateTime.Now.Day > PlayerPrefs.GetInt("LastLogInDay"))
        {
            Debug.Log("Resetting!");
            Debug.Log($"{DateTime.Now.Year}/{DateTime.Now.Month}/{DateTime.Now.Day}");
            Debug.Log($"{PlayerPrefs.GetInt("LastLogInYear")}/{PlayerPrefs.GetInt("LastLogInMonth")}/{PlayerPrefs.GetInt("LastLogInDay")}");
            Reset();
        }
    }

    public void Reset()
    {
        PlayerPrefs.SetInt("TodaySolved", 0);
        AlreadyAttempted = false;
        _dailyButton.interactable = true;
        PlayerPrefs.SetInt("LastLogInYear", DateTime.Now.Year);
        PlayerPrefs.SetInt("LastLogInMonth", DateTime.Now.Month);
        PlayerPrefs.SetInt("LastLogInDay", DateTime.Now.Day);
    }

    public void SetDailyMode(bool isDailyMode)
    {
        if (isDailyMode)
        {
            PlayerPrefs.SetInt("TodaySolved", 1);
            AlreadyAttempted = true;
            Initialize();
        }
        IsDailyMode = isDailyMode;
    }

    public void IncreaseDifficultyLevel()
    {
        CurrentLevel++;
        if (CurrentLevel >= 5)
        {
            CurrentLevel = 5;
            _increaseDifficultyButton.interactable = false;
        }
        else
        {
            _increaseDifficultyButton.interactable = true;
            _decreaseDifficultyButton.interactable = true;
        }

        SetDifficultyLevel(CurrentLevel);
    }

    public void DecreaseDifficultyLevel()
    {
        CurrentLevel--;
        if (CurrentLevel <= 1)
        {
            CurrentLevel = 1;
            _decreaseDifficultyButton.interactable = false;
        }
        else
        {
            _decreaseDifficultyButton.interactable = true;
            _increaseDifficultyButton.interactable = true;
        }

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
    }

    private void SetDifficultyDescription()
    {
        _difficultyText.text = $"Difficulty: {CurrentLevel}\n" +
                              $"Solve Time: {SolveTime} seconds\n" +
                              $"Letter Guesses: {LetterGuesses}\n" +
                              $"Word Guesses: {WordGuesses}\n" +
                              $"Vowel Guesses: {VowelGuesses}";
    }
}
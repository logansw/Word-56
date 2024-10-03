using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
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
}
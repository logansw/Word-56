using System;
using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Static
    public static GameManager s_instance;
    // Public
    public int CurrentRound;
    public List<int> LetterPurchases;
    public List<int> TimeElapses;
    public List<int> SolvePurchases;
    public List<int> SelectionBonuses;
    public List<int> TimeBonuses;
    [SerializeField] private GameObject _screenMain;
    [SerializeField] private GameObject[] _panels;

    void Awake() {
        s_instance = this;
    }

    void Start() {
        CurrentRound = 1;
        LetterPurchases = new List<int>();
        TimeElapses = new List<int>();
        SolvePurchases = new List<int>();
        SelectionBonuses = new List<int>();
        TimeBonuses = new List<int>();
    }

    public void Continue()
    {
        LetterPurchases.Add(WordManager.s_instance.LetterPurchases);
        TimeElapses.Add(WordManager.s_instance.TimeElapsed);
        SolvePurchases.Add(WordManager.s_instance.SolvePurchases);
        SelectionBonuses.Add(WordManager.s_instance.SelectionBonus);
        TimeBonuses.Add(WordManager.s_instance.TimeBonus);
        CurrentRound++;
        StateController.s_instance.ChangeState(StateController.s_instance.PreStartState);
    }

    public void Finish()
    {
        StateController.s_instance.ChangeState(StateController.s_instance.GameOverState);
    }

    public int GetFinalScore() {
        int finalScore = 0;
        for (int i = 0; i < ConfigurationManager.s_instance.SeriesLength; i++) {
            finalScore += ConfigurationManager.s_instance.StartingScore;
            finalScore -= LetterPurchases[i];
            finalScore -= SolvePurchases[i];
        }
        return finalScore;
    }

    public void SetActivePanel(int index)
    {
        foreach (GameObject panel in _panels)
        {
            panel.SetActive(false);
        }
        _panels[index].SetActive(true);
    }

    public bool IsLastRound()
    {
        return CurrentRound == ConfigurationManager.s_instance.SeriesLength;
    }
}
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
    public List<int> TimePenalties;
    public List<int> SolvePurchases;
    [SerializeField] private GameObject _screenMain;

    void Awake() {
        s_instance = this;
    }

    void Start() {
        CurrentRound = 1;
        LetterPurchases = new List<int>();
        TimePenalties = new List<int>();
        SolvePurchases = new List<int>();
    }

    public void Continue() {
        LetterPurchases.Add(WordManager.s_instance.LetterPurchases);
        TimePenalties.Add(WordManager.s_instance.TimePenalty);
        SolvePurchases.Add(WordManager.s_instance.SolvePurchases);
        CurrentRound++;
        if (CurrentRound > ConfigurationManager.s_instance.SeriesLength) {
            StateController.s_instance.ChangeState(StateController.s_instance.GameOverState);
        } else {
            StateController.s_instance.ChangeState(StateController.s_instance.PreStartState);
        }
    }

    public int GetFinalScore() {
        int finalScore = 0;
        for (int i = 0; i < ConfigurationManager.s_instance.SeriesLength; i++) {
            finalScore += WordManager.s_instance.RoundScore;
            finalScore -= LetterPurchases[i];
            finalScore -= TimePenalties[i];
            finalScore -= SolvePurchases[i];
        }
        return finalScore;
    }
}
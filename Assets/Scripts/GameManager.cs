using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Static
    public static GameManager s_instance;
    // Public
    public int CurrentRound;
    public GameState State;
    public int Score;

    void Awake() {
        s_instance = this;
        State = GameState.PreStart;
    }

    private void StartSeries() {
        CurrentRound = 1;
    }

    public void StartRound() {
        Score = 30000;
        State = GameState.Guess;
        WordManager wordManager = GameObject.Find("WordManager").GetComponent<WordManager>();
        if (ConfigurationManager.s_instance.ChallengeMode) {
            wordManager.ChooseWords(Random.Range(5, 6));
        } else {
            wordManager.ChooseWords(Random.Range(3, 4));
        }
    }
}

public enum GameState {
    PreStart,
    Guess,
    Solve,
    GameOver
}
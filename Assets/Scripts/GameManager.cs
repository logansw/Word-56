using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Static
    public static GameManager s_instance;
    // Public
    public int CurrentRound;
    public int Score;
    public int LettersPurchased;

    void Awake() {
        s_instance = this;
    }

    private void StartSeries() {
        CurrentRound = 1;
    }

    public void StartRound() {
        StateController.s_instance.ChangeState(StateController.s_instance.BuyState);
    }
}
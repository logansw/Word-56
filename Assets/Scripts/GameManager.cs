using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Static
    public static GameManager s_instance;
    // Public
    public int CurrentRound;

    void Awake() {
        s_instance = this;
    }

    private void StartSeries() {
        CurrentRound = 1;
    }

    public void StartRound() {
        WordManager wordManager = GameObject.Find("WordManager").GetComponent<WordManager>();
        if (ConfigurationManager.s_instance.ChallengeMode) {
            wordManager.ChooseWords(Random.Range(5, 6));
        } else {
            wordManager.ChooseWords(Random.Range(3, 4));
        }
        Debug.Log(wordManager.WordA);
        Debug.Log(wordManager.WordB);
    }
}

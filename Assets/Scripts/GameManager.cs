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
    [SerializeField] private GameObject _screenMain;
    [SerializeField] private GameObject[] _panels;

    void Awake() {
        s_instance = this;
    }

    void Start() {
        CurrentRound = 1;
    }

    public void Continue()
    {
        CurrentRound++;
        StateController.s_instance.ChangeState(StateController.s_instance.PreStartState);
    }

    public void Finish()
    {
        StateController.s_instance.ChangeState(StateController.s_instance.GameOverState);
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
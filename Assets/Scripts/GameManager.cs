using System;
using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Static
    public static GameManager s_instance;
    // Public
    [SerializeField] private GameObject _screenMain;
    [SerializeField] private GameObject[] _panels;

    void Awake() {
        s_instance = this;
    }

    public void Continue()
    {
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
}
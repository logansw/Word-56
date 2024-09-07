using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreStartState : State
{
    [SerializeField] private GameObject _screenMain;

    public PreStartState()
    {
        GameState = StateType.PreStart;
    }

    public override void OnEnter(StateController stateController)
    {
        _screenMain.SetActive(true);
        WordManager wordManager = GameObject.Find("WordManager").GetComponent<WordManager>();
        wordManager.Initialize();
        wordManager.Reset();
        wordManager.ChooseWords();
    }
    public override void UpdateState(StateController stateController)
    {
        // Do nothing
    }
    public override void OnExit(StateController stateController)
    {
        // Do nothing
    }
}
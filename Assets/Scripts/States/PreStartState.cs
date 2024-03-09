using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreStartState : State
{
    [SerializeField] private Button _startbutton;
    [SerializeField] private GameObject _screenMain;

    public PreStartState() {
        GameState = StateType.PreStart;
    }
    public override void OnEnter(StateController stateController)
    {
        _screenMain.SetActive(true);
        _startbutton.interactable = true;
        WordManager wordManager = GameObject.Find("WordManager").GetComponent<WordManager>();
        wordManager.Initialize();
        wordManager.Reset();
        if (ConfigurationManager.s_instance.ChallengeMode) {
            wordManager.ChooseWords(Random.Range(5, 6));
        } else {
            wordManager.ChooseWords(Random.Range(3, 4));
        }
    }
    public override void UpdateState(StateController stateController)
    {
        // Nothing
    }
    public override void OnExit(StateController stateController)
    {
        _startbutton.interactable = false;
    }
}

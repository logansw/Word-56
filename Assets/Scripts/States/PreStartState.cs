using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreStartState : State
{
    public PreStartState() {
        GameState = StateType.PreStart;
    }
    public override void OnEnter(StateController stateController)
    {
        GameManager.s_instance.Score = 30000;
        GameManager.s_instance.LettersPurchased = 0;
        WordManager wordManager = GameObject.Find("WordManager").GetComponent<WordManager>();
        wordManager.InitializeWords();
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
        // Nothing
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    public StateType GameState;
    public virtual void OnEnter(StateController stateController)
    {

    }
    public virtual void UpdateState(StateController stateController)
    {

    }
    public virtual void OnExit(StateController stateController)
    {

    }

    [System.Serializable]
    public enum StateType {
        PreStart,
        Buy,
        Solve,
        GameOver,
        Intermission
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the state of the game.
/// </summary>
public class StateController : MonoBehaviour
{
    // Static
    public static StateController s_instance;
    // Internal
    public State CurrentState;
    // States
    public PreStartState PreStartState = new PreStartState();
    public BuyState BuyState = new BuyState();
    public SolveState SolveState = new SolveState();
    public GameOverState GameOverState = new GameOverState();

    void Awake() {
        s_instance = this;
    }

    void Start() {
        ChangeState(PreStartState);
    }

    void Update()
    {
        CurrentState.UpdateState(this);
    }

    /// <summary>
    /// Changes the current state of the game.
    /// </summary>
    /// <param name="newState">The new state to change to.</param>
    public void ChangeState(State newState)
    {
        if (CurrentState != null) {
            CurrentState.OnExit(this);
        }
        CurrentState = newState;
        CurrentState.OnEnter(this);
    }

    public static State.StateType GetCurrentState() {
        return s_instance.CurrentState.GameState;
    }
}

public class State
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
        GameOver
    }
}
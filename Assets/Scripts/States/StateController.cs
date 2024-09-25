using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    public PreStartState PreStartState;
    public BuyState BuyState;
    public SolveState SolveState;
    public GameOverState GameOverState;
    public DefeatState DefeatState;
    public VictoryState VictoryState;
    private State _previousState;

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
        _previousState = CurrentState;
        CurrentState = newState;
        CurrentState.OnEnter(this);
    }

    public static State.StateType GetCurrentState() {
        return s_instance.CurrentState.GameState;
    }

    public void ChangeToPreviousState() {
        ChangeState(_previousState);
    }
}
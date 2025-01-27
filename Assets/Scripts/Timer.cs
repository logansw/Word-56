using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer s_instance;
    [SerializeField] private TMP_Text _text;
    private Coroutine _updateCoroutine;
    private int _secondsRemaining;

    void Awake()
    {
        s_instance = this;
    }

    void OnEnable()
    {
        _text.color = Color.black;
        Resume();
    }

    void OnDisable()
    {
        PauseTimer();
    }

    public void StartTimer()
    {
        _secondsRemaining = ConfigurationManager.s_instance.SolveTime;
        _updateCoroutine = StartCoroutine(UpdateStopwatch());
    }

    public void StartTimer(int time)
    {
        if (_updateCoroutine != null)
        {
            StopCoroutine(_updateCoroutine);
        }
        _secondsRemaining = time;
        _updateCoroutine = StartCoroutine(UpdateStopwatch());
    }

    public void Resume()
    {
        if (StateController.GetCurrentState() == State.StateType.Buy || StateController.GetCurrentState() == State.StateType.Solve)
        {
            StartTimer(_secondsRemaining);
        }
    }

    public void PauseTimer()
    {
        if (_updateCoroutine != null)
        {
            StopCoroutine(_updateCoroutine);
        }
    }

    public void StopTimer()
    {
        if (_updateCoroutine != null)
        {
            StopCoroutine(_updateCoroutine);
            _text.text = "";
        }
    }

    private IEnumerator UpdateStopwatch()
    {
        while (true)
        {
            int minutes = _secondsRemaining / 60;
            int seconds = _secondsRemaining % 60;
            _text.text = minutes.ToString("00") + ":" + seconds.ToString("00");
            yield return new WaitForSeconds(1);
            _secondsRemaining--;
            if (_secondsRemaining <= 0)
            {
                _text.text = "00:00";
                HighscoreWriter.s_Instance.SetOutcomeText(false, WordManager.s_instance.IsSecondChance);
                HighscoreWriter.s_Instance.ShowHighscoreEntry();
                StateController.s_instance.ChangeState(StateController.s_instance.DefeatState);
                WordManager.s_instance.FreezeGame();
                yield break;
            }
            else if (_secondsRemaining == 15)
            {
                StartCoroutine(Blink());
            }
        }
    }

    private IEnumerator Blink()
    {
        _text.color = Color.white;
        yield return new WaitForSecondsRealtime(0.25f);
        _text.color = Color.red;
        yield return new WaitForSecondsRealtime(0.25f);
        _text.color = Color.white;
        yield return new WaitForSecondsRealtime(0.25f);
        _text.color = Color.red;
        yield return new WaitForSecondsRealtime(0.25f);
        _text.color = Color.black;
    }
}

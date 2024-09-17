using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer s_instance;    
    private const int SOLVE_TIME = 120;
    [SerializeField] private TMP_Text _text;
    private Coroutine _updateCoroutine;
    private int _secondsRemaining;

    void Awake()
    {
        s_instance = this;
    }

    void OnDisable()
    {
        StopTimer();
    }

    public void StartTimer()
    {
        _secondsRemaining = SOLVE_TIME;
        _updateCoroutine = StartCoroutine(UpdateStopwatch());
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
                StateController.s_instance.ChangeState(StateController.s_instance.DefeatState);
                yield break;
            }
        }
    }
}

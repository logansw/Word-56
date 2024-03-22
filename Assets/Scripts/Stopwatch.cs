using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Stopwatch : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private DateTime _startTime;
    private Coroutine _updateCoroutine;

    void OnDisable()
    {
        StopStopwatch();
    }

    public void StartStopwatch()
    {
        _startTime = DateTime.Now;
        _updateCoroutine = StartCoroutine(UpdateStopwatch());
    }

    public void StopStopwatch()
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
            TimeSpan timeSpan = DateTime.Now - _startTime;
            _text.text = timeSpan.ToString(@"mm\:ss");
            yield return null;
        }
    }
}

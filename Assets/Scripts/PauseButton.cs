using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private GameObject _obfuscator;
    [SerializeField] private GameObject _mainScreen;

    void OnDisable()
    {

    }

    public void Pause()
    {
        Time.timeScale = 0f;
        _obfuscator.SetActive(true);
        _mainScreen.SetActive(false);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        _obfuscator.SetActive(false);
        _mainScreen.SetActive(true);
    }
}

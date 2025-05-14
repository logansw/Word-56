using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    void OnEnable()
    {
        SceneMan.e_OnSceneLoaded += OnSceneLoaded;
        SceneMan.e_OnSceneUnloaded += OnSceneUnloaded;
    }

    void OnDisable()
    {
        SceneMan.e_OnSceneLoaded -= OnSceneLoaded;
        SceneMan.e_OnSceneUnloaded -= OnSceneUnloaded;
    }

    void OnSceneLoaded(string sceneName)
    {
        if (sceneName == "Scores")
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }

    void OnSceneUnloaded(string sceneName)
    {
        if (sceneName == "Scores")
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
    }
}

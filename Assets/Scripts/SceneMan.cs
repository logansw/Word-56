using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMan : MonoBehaviour
{
    public delegate void SceneLoadedDelegate(string sceneName);
    public static SceneLoadedDelegate e_OnSceneLoaded;
    public delegate void SceneUnloadedDelegate(string sceneName);
    public static SceneUnloadedDelegate e_OnSceneUnloaded;

    public static void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        e_OnSceneLoaded?.Invoke(sceneName);
    }

    public static void LoadSceneAdditively(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        e_OnSceneLoaded?.Invoke(sceneName);
    }

    public static void UnloadSceneAdditively(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
        e_OnSceneUnloaded.Invoke(sceneName);
    }
}

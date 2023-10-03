using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMan : MonoBehaviour
{
    public static void LoadScene(string sceneName) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}

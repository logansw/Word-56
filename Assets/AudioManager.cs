using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager s_instance;
    public AudioSource Negative;
    public AudioSource Positive;
    public AudioSource Click;
    public AudioSource Victory;
    public AudioSource GameOver;
    public AudioSource TimeWarning;

    void Awake() {
        if (s_instance != null) {
            Destroy(s_instance.gameObject);
        }
        s_instance = this;
        DontDestroyOnLoad(gameObject);
    }
}

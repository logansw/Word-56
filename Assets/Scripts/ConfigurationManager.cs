using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurationManager : MonoBehaviour
{
    // Static
    public static ConfigurationManager s_instance;

    // Public
    public bool ChallengeMode;
    public int SeriesLength;

    void Awake() {
        s_instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetChallengeMode(bool enabled) {
        ChallengeMode = enabled;
    }

    public void SetSeriesLength(int numberOfRounds) {
        SeriesLength = numberOfRounds;
    }
}

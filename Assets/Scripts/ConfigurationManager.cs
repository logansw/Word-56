using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurationManager : MonoBehaviour
{
    // Static
    public static ConfigurationManager s_instance;

    // Public
    public bool ConsecutiveVowelsAllowed;

    // External References
    [SerializeField] private UnityEngine.UI.Image[] _singleButtons;
    [SerializeField] private UnityEngine.UI.Image[] _tripleButtons;
    [SerializeField] private UnityEngine.UI.Image[] _pentaButtons;
    [SerializeField] private UnityEngine.UI.Image[] _regularButtons;
    [SerializeField] private UnityEngine.UI.Image[] _challengeButtons;

    void Awake() {
        if (s_instance != null) {
            Destroy(s_instance.gameObject);
        }
        s_instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
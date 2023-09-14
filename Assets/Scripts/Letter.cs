using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Letter : MonoBehaviour
{
    public char Character;
    public PhonemicType Type => (Character == 'A' || Character == 'E' || Character == 'I' || Character == 'O' || Character == 'U') ? PhonemicType.Vowel : PhonemicType.Consonant;
    public LetterState State;
    public int Cost;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private BoxCollider2D _collider;

    public delegate void LetterClicked(Letter letter);
    public static LetterClicked e_OnLetterClicked;

    public void Initialize() {
        _text.text = Character.ToString();
    }

    void Update() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (_collider.bounds.IntersectRay(ray)) {
                    e_OnLetterClicked?.Invoke(this);
                }
            }
        }
    }
}

public enum PhonemicType {
    Vowel,
    Consonant
}

public enum LetterState {
    Default,
    Disabled,
    Correct
}
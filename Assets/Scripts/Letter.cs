using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Letter : MonoBehaviour
{
    public char Character;
    public PhonemicType Type => (Character == 'A' || Character == 'E' || Character == 'I' || Character == 'O' || Character == 'U') ? PhonemicType.Vowel : PhonemicType.Consonant;
    public LetterState LetterState;
    public int Cost;
    public bool Purchased;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    // Letter Costs
    private static Dictionary<char, int> s_consonantCostMap = new Dictionary<char, int>();
    private static bool s_dictionaryInitialized;
    private char[] _setOne = new char[] {'J', 'X', 'Q', 'Z'};
    private char[] _setTwo = new char[] {'K', 'V', 'W'};
    private char[] _setThree = new char[] {'Y', 'F', 'H'};
    private char[] _setFour = new char[] {'P', 'C', 'M', 'B'};
    private char[] _setFive = new char[] {'D', 'G', 'N'};
    private char[] _setSix = new char[] {'L', 'R', 'S', 'T'};
    private char[] _vowels = new char[] {'A', 'E', 'I', 'O', 'U'};

    public delegate void LetterClicked(Letter letter);
    public static LetterClicked e_OnLetterClicked;
    public int InitialVowelCost = 300;

    void Awake() {
        if (!s_dictionaryInitialized) {
            RegisterLetterCosts(_setOne, 200);
            RegisterLetterCosts(_setTwo, 225);
            RegisterLetterCosts(_setThree, 250);
            RegisterLetterCosts(_setFour, 275);
            RegisterLetterCosts(_setFive, 300);
            RegisterLetterCosts(_setSix, 325);
            s_dictionaryInitialized = true;
        }
    }

    private void RegisterLetterCosts(char[] set, int cost) {
        foreach (char c in set) {
            s_consonantCostMap.Add(c, cost);
        }
    }

    void Start() {
        Initialize();
    }

    public void Initialize() {
        WordManager.s_instance.Letters.Add(this);
        _text.text = Character.ToString();
        if (Type == PhonemicType.Vowel) {
            LetterState = LetterState.Disabled;
        } else {
            LetterState = LetterState.Default;
        }
        if (s_consonantCostMap.ContainsKey(Character)) {
            Cost = s_consonantCostMap[Character];
        } else if (IsVowel()){
            Cost = InitialVowelCost;
        }
        _costText.text = Cost.ToString();
        RenderLetter();
    }

    public void SetCostsToFaceValue() {
        if (s_consonantCostMap.ContainsKey(Character)) {
            Cost = s_consonantCostMap[Character];
        }
    }

    public void Reset() {
        Purchased = false;
        if (Type == PhonemicType.Vowel) {
            LetterState = LetterState.Disabled;
        } else {
            LetterState = LetterState.Default;
        }
        if (s_consonantCostMap.ContainsKey(Character)) {
            Cost = s_consonantCostMap[Character];
        } else if (IsVowel()){
            Cost = InitialVowelCost;
        }
        _costText.text = Cost.ToString();
        RenderLetter();
    }

    void Update() {
        if (StateController.GetCurrentState() == State.StateType.PreStart || StateController.GetCurrentState() == State.StateType.GameOver) {
            return;
        }

        HandleTouch();
        HandleKeypress();
    }

    private void HandleKeypress() {
        if (Input.GetKeyDown(Character.ToString().ToLower())) {
            // Debug.Log("Click!");
            HandleLetterClicked();
        }
    }

    private void HandleTouch() {
        // Process touch input and return early if no touch is detected on this letter
        if (Input.touchCount <= 0) { return; }
        Touch touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began) { return; }
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        if (!_collider.bounds.IntersectRay(ray)) { return; }

        HandleLetterClicked();
    }

    private void HandleLetterClicked() {
        if (StateController.GetCurrentState() == State.StateType.Buy) {
            if (LetterState.Equals(LetterState.Default)) {
                e_OnLetterClicked?.Invoke(this);
            } else if (LetterState.Equals(LetterState.Disabled)) {
                // Play dull sound
                // Inform the player why this letter cannot be pressed
            } else {
                // Play dull sound
                // Inform the player why this letter cannot be pressed
            }
        } else if (StateController.GetCurrentState() == State.StateType.Solve) {
            if (LetterState.Equals(LetterState.Default) || LetterState.Equals(LetterState.Disabled)) {
                e_OnLetterClicked?.Invoke(this);
            } else if (LetterState.Equals(LetterState.Incorrect) || LetterState.Equals(LetterState.Correct)) {
                // Play dull sound
                // Inform the player why this letter cannot be pressed
            }
        }
    }

    public void RenderLetter() {
        switch (LetterState) {
            case LetterState.Default:
                _spriteRenderer.color = Color.white;
                break;
            case LetterState.Disabled:
                _spriteRenderer.color = Color.gray;
                break;
            case LetterState.Correct:
                _spriteRenderer.color = Color.green;
                break;
            case LetterState.Incorrect:
                _spriteRenderer.color = Color.black;
                break;
        }
        _costText.text = Cost.ToString();
    }

    public void ColorLetterForSelection() {
        _spriteRenderer.color = Color.yellow;
    }

    public bool IsVowel() {
        return Character == 'A' || Character == 'E' || Character == 'I' || Character == 'O' || Character == 'U';
    }

    public void IncreaseCost(int amount) {
        Cost += amount;
        RenderLetter();
    }

    public void SetCost(int amount)
    {
        Cost = amount;
        RenderLetter();
    }
}

public enum PhonemicType {
    Vowel,
    Consonant
}

public enum LetterState {
    Default,
    Disabled,
    Correct,
    Incorrect
}
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Letter : MonoBehaviour
{
    public char Character;
    public PhonemicType Type => (Character == 'A' || Character == 'E' || Character == 'I' || Character == 'O' || Character == 'U') ? PhonemicType.Vowel : PhonemicType.Consonant;
    public LetterState LetterState;
    public bool Purchased;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _cooldownMask;
    private bool _onCooldown;

    // Letter Costs
    public delegate void LetterClicked(Letter letter);
    public static LetterClicked e_OnLetterClicked;
    public static bool ResetCooldowns;

    void Start() {
        Initialize();
    }

    void OnEnable()
    {
        e_OnLetterClicked += SetLetterCooldown;
        DefeatState.e_OnDefeat += EndCooldown;
    }

    void OnDisable()
    {
        e_OnLetterClicked -= SetLetterCooldown;
        DefeatState.e_OnDefeat -= EndCooldown;
    }

    public void Initialize() {
        WordManager.s_instance.Letters.Add(this);
        _text.text = Character.ToString();
        if (Type == PhonemicType.Vowel) {
            LetterState = LetterState.Disabled;
        } else {
            LetterState = LetterState.Default;
        }
        RenderLetter();
        _onCooldown = false;
    }

    private void SetLetterCooldown(Letter letter)
    {
        StartCoroutine(Cooldown());
    }

    public void Reset() {
        _onCooldown = false;
        Purchased = false;
        if (Type == PhonemicType.Vowel) {
            LetterState = LetterState.Disabled;
        } else {
            LetterState = LetterState.Default;
        }
        RenderLetter();
    }

    void Update() {
        if (ResetCooldowns)
        {
            EndCooldown();
            StartCoroutine(DelayedResetCooldowns());
        }

        if (StateController.GetCurrentState() == State.StateType.GameOver || _onCooldown) {
            return;
        }

        HandleTouch();
    }

    private IEnumerator DelayedResetCooldowns()
    {
        yield return null;
        ResetCooldowns = false;
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
        if (StateController.GetCurrentState() == State.StateType.PreStart) {
            WordManager.s_instance.StartRound();
            Timer.s_instance.StartTimer();
            e_OnLetterClicked?.Invoke(this);
        } else if (StateController.GetCurrentState() == State.StateType.Buy) {
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
        if (StateController.GetCurrentState() == State.StateType.Solve)
        {
            switch (LetterState) {
                case LetterState.Default:
                    _spriteRenderer.color = Color.white;
                    break;
                case LetterState.Disabled:
                    _spriteRenderer.color = Color.white;
                    break;
                case LetterState.Correct:
                    _spriteRenderer.color = Color.black;
                    break;
                case LetterState.Incorrect:
                    _spriteRenderer.color = Color.black;
                    break;
            }
        }
        else
        {
            switch (LetterState)
            {
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
        }
    }

    public void ColorLetterForSelection() {
        _spriteRenderer.color = Color.yellow;
    }

    public bool IsVowel() {
        return Character == 'A' || Character == 'E' || Character == 'I' || Character == 'O' || Character == 'U';
    }

    private IEnumerator Cooldown()
    {
        if (StateController.GetCurrentState() != State.StateType.Buy && StateController.GetCurrentState() != State.StateType.PreStart)
        {
            yield break;
        }
        _onCooldown = true;
        float t = 0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime;
            _cooldownMask.transform.localScale = new Vector2(1, Mathf.Lerp(1, 0, t));
            yield return null;
        }
        EndCooldown();
    }

    private void EndCooldown()
    {
        _onCooldown = false;
        _cooldownMask.transform.localScale = new Vector2(1, 0);
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
    Incorrect,
}
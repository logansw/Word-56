using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SolveLetter : MonoBehaviour
{
    public char Character { get; private set; }
    public SolveStatus Status { get; private set; }
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private TMP_Text _text;
    public static Action e_OnLetterClicked;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    void Update()
    {
        if (StateController.GetCurrentState() != State.StateType.Solve || Status.Equals(SolveStatus.Correct)) { return; }
        HandleTouch();
    }
    
    private void HandleTouch()
    {
        // Process touch input and return early if no touch is detected on this letter
        if (Input.touchCount <= 0) { return; }
        Touch touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began) { return; }
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        if (!_collider.bounds.IntersectRay(ray)) { return; }

        HandleLetterClicked();
    }

    private void HandleLetterClicked()
    {
        if (WordManager.s_instance.SelectedLetter == '\0')
        {
            return;
        }
        else
        {
            SetGuess(WordManager.s_instance.SelectedLetter);
        }
    }

    public void SetCorrect(char c)
    {
        Character = c;
        _text.text = Character.ToString();
        Status = SolveStatus.Correct;
        _spriteRenderer.color = Color.white;
    }

    public void Clear()
    {
        Character = '\0';
        _text.text = "";
        Status = SolveStatus.Blank;
        _spriteRenderer.color = new Color(1, 1, 1, 80f/255f);
    }

    public void SetGuess(char c)
    {
        Character = c;
        _text.text = Character.ToString();
        Status = SolveStatus.Guess;
        e_OnLetterClicked?.Invoke();
    }
}

public enum SolveStatus
{
    Blank,
    Correct,
    Guess
}
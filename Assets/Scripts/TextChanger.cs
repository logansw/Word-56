using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.TextCore.Text;

public class TextChanger : MonoBehaviour
{
    [SerializeField] private TMP_Text _textElement;
    private Coroutine _restoreTextCoroutine;
    
    public void SetTextTemporary(string newText)
    {
        string oldText = _textElement.text;
        _textElement.text = newText;
        if (_restoreTextCoroutine == null)
        {
            _restoreTextCoroutine = StartCoroutine(RestoreText(oldText, 1.0f));
        }
    }

    private IEnumerator RestoreText(string text, float duration)
    {
        yield return new WaitForSeconds(duration);
        _textElement.text = text;
    }
}

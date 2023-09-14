using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardGenerator : MonoBehaviour
{
    [SerializeField] private Letter _letterPrefab;

    void Start() {
        GenerateLetters();
    }

    private void GenerateLetters() {
        for (int i = 0; i < 26; i++) {
            Vector2 pos = new Vector2(i % 8, -i / 8);
            Letter letter = Instantiate(_letterPrefab, transform);
            letter.transform.localPosition = pos;
            letter.Character = (char)('A' + i);
            letter.Initialize();
        }
    }
}

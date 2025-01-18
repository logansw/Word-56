using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private List<Image> _buttonImages;

    void Start()
    {
        int currentLevel = PlayerPrefs.GetInt("DifficultyLevel", 1);
        HighlightButton(currentLevel-1);
    }

    public void HighlightButton(int index)
    {
        foreach (Image image in _buttonImages)
        {
            image.color = Color.white;
        }
        _buttonImages[index].color = Color.yellow;
    }
}
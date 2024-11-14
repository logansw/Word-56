using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnablerDisabler : MonoBehaviour
{
    private Button _button;
    
    public void SetButtonEnabled(bool enabled)
    {
        _button = GetComponent<Button>();
        gameObject.SetActive(enabled);
        _button.interactable = enabled;
        if (enabled)
        {
            _button.transform.localScale = Vector3.one;
        }
        else
        {
            _button.transform.localScale = Vector3.zero;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI _timerText = null;


    private void Awake()
    {
        _timerText = this.GetComponent<TextMeshProUGUI>();
        if (_timerText == null)
        {
            Debug.Log($"OH NO NO TIMER NOT FOUND");
        }
    }

    public void SetText(string text)
    {
        _timerText.text = text;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour
{
    public event Action<string> OnButtonClicked;  
    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(HandleClick);
    }

    void HandleClick()
    {
        string name = GetComponentInChildren<TextMeshProUGUI>().text;
        Debug.Log(name);
        OnButtonClicked?.Invoke(name);
    }
}

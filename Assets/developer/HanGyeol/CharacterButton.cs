using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CharacterButton : MonoBehaviour
{
    public static event Action OnCharacterButtonClicked;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            // 버튼 클릭 시 이벤트 발사 
            OnCharacterButtonClicked?.Invoke();
        });
    }
} 

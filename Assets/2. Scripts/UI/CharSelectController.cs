using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSelectController : MonoBehaviour
{
    private int selected = 0;
    [SerializeField] private List<Image> checkImages = new List<Image>();
    private void Awake()
    {
        UIManager.UIInstance.RegistCharSelectPanel(gameObject);
    }

    private void OnEnable()
    {
        foreach(var image in checkImages)
        {
            image.enabled = false;
        }
        checkImages[GameManager.Instance.SelectedPlayerType].enabled = true;
    }

    public void OnBackButtonClick()
    {
        ClosePanel();
    }

    public void OnCharacterButtonClick(int index)
    {
        GameManager.Instance.SetPlayerCharacter(index);
        ClosePanel();
    }

    private void ClosePanel()
    {
        UIManager.UIInstance.CloseCharSelectPanel();
    }
}

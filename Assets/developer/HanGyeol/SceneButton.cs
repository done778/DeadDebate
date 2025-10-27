using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
public class SceneButton : MonoBehaviour
{
    [SerializeField] string SceneName;

    public void OnClick()
    {
       
        UIManager.UIInstance.LoadScene(SceneName);
    }
}

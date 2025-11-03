using UnityEngine;
public class SceneButton : MonoBehaviour
{
    [SerializeField] string SceneName;

    public void OnClick()
    {
       
        UIManager.UIInstance.LoadScene(SceneName);
    }
}

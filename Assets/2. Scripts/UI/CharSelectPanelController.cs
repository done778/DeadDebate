using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelectPanelController : MonoBehaviour
{
    private void Awake()
    {
        UIManager.UIInstance.RegistCharSelectPanel(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

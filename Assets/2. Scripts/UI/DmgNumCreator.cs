using TMPro;
using UnityEngine;

public class DmgNumCreator : MonoBehaviour
{
    // 텍스트 오브젝트 풀링
    private int DMG_TEXT_ARR_SIZE = 20;
    [SerializeField] private int index;

    public TextMeshProUGUI dmgText;
    private TextMeshProUGUI[] dmgTextArr;
    private Camera mainCamera;
    private Vector3 screenPosition;

    private void Awake()
    {
        GameManager.Instance.RegistDmgNumCreator(this);
    }

    private void Start()
    {
        mainCamera = Camera.main;
        index = 0;
        dmgTextArr = new TextMeshProUGUI[DMG_TEXT_ARR_SIZE];
        for (int i = 0; i < DMG_TEXT_ARR_SIZE; i++)
        {
            dmgTextArr[i] = Instantiate(dmgText, transform);
            dmgTextArr[i].gameObject.SetActive(false);
        }
    }
    public void DamageTextCreate(Vector3 position, int dmg)
    {
        screenPosition = mainCamera.WorldToScreenPoint(position);
        dmgTextArr[index].transform.position = screenPosition;
        dmgTextArr[index].text = dmg.ToString();
        dmgTextArr[index].gameObject.SetActive(true);
        index++;
        if (index >= DMG_TEXT_ARR_SIZE)
        {
            index = 0;
        }
    }
}

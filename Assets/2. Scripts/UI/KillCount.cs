using UnityEngine;
using TMPro;

public class KillCount : MonoBehaviour
{
    [SerializeField] TMP_Text KillText;
    private int killCount = 0;

    private void Start()
    {
        KillText.text = "kill : 0";

        if (GameManager.Instance != null )
        {
            // 적이 처치된 이벤트가 발생하면 킬카운트++ 한다.
            EnemyPool.Instance.OnDeath += UpdateKillText;
        }
    }

    private void OnDestroy()
    {
        if ( GameManager.Instance != null )  //중복방지
        {
            EnemyPool.Instance.OnDeath -= UpdateKillText;
        }
    }

    void UpdateKillText()
    {
        killCount++;  // 이벤트 발생 시마다 카운트 증가 + 텍스트 갱신
        KillText.text = $"Kill : {killCount}";
    }

}

using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] Image HpIndicator;
    private PlayerController player;

    public void Init(GameObject playerInfo)
    {
        player = playerInfo.GetComponent<PlayerController>();
        player.OnHpChanged += HpUIUpdate;
    }

    private void OnDestroy()
    {
        player.OnHpChanged -= HpUIUpdate;
    }

    private void HpUIUpdate(int cur, int max)
    {
        HpIndicator.fillAmount = (float)cur / max;
    }
}

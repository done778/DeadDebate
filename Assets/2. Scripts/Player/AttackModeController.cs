using UnityEngine;

public class AttackModeController : MonoBehaviour
{
    private IAttackMode currentMode; // 현재 공격모드
    private AutoBulletMode autoMode; // 자동공격
    private HandleBulletMode handleMode; // 조작공격

    private void Start()
    {
        autoMode = gameObject.AddComponent<AutoBulletMode>();
        handleMode = gameObject.AddComponent<HandleBulletMode>();
        
        handleMode.enabled = false;
        currentMode = autoMode; // 기본 자동공격
    }

    public IAttackMode GetCurrentMode() => currentMode;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if ((object)currentMode == (object)autoMode)
            {
                currentMode = handleMode;
                autoMode.enabled = false;
                handleMode.enabled = true;
                // Debug.Log("HandleBulletMode");
            }
            else
            {
                currentMode = autoMode;
                handleMode.enabled = false;
                autoMode.enabled = true;
                // Debug.Log("AutoBulletMode");
            }
        }
    }
}

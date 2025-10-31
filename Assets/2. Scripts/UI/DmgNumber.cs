using UnityEngine;

public class DmgNumber : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float remainTime = 1f;
    private float elapsedTime = 0f;

    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime * moveSpeed;
        elapsedTime += Time.deltaTime;
        if (elapsedTime > remainTime)
        {
            elapsedTime = 0f;
            gameObject.SetActive(false);
        }
    }
}

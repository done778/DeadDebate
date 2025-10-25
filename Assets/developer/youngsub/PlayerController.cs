using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

[System.Serializable]
public struct PlayerStat
{
    public float healthPoint;
    public int attakPower;
    public float moveSpeed;
    public float attackSpeed;
}

public class PlayerController : MonoBehaviour, IMovement, IAttack
{
    [SerializeField] private PlayerStat stat;
    private Rigidbody rb;

    [SerializeField] private float rotateSpeed;
    private Vector3 direction = Vector3.zero;
    private Renderer rend;
    private bool isInvincibilityl;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
    }

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (direction != Vector3.zero)
        {
            Move(direction);
            LookRotate(direction);
        }
    }

    #region method
    private void Initialize()
    {
        //rb.isKinematic = true;
    }

    public void Attack()
    {

    }

    IEnumerator TakeDamage()
    {
        isInvincibilityl = true;
        yield return new WaitForSeconds(0.2f);
        rend.material.color = Color.white;
        yield return new WaitForSeconds(0.3f);
        isInvincibilityl = false;
    }

    public void TakeDamage(float damage)
    {
        if (isInvincibilityl) return;

        stat.healthPoint -= damage;
        if (stat.healthPoint < 0)
        {
            stat.healthPoint = 0;
            Time.timeScale = 0;
        }
        rend.material.color = Color.red;
        StartCoroutine(TakeDamage());
    }

    public void LookRotate(Vector3 dir)
    {
        Quaternion rotation = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
    }

    public void Move(Vector3 dir)
    {
        transform.Translate(Vector3.forward * stat.moveSpeed * Time.deltaTime);
    }
    #endregion
}

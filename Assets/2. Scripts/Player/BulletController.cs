using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int damage;
    [SerializeField] private float bulletSpeed;
   
    void Start()
    {
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * bulletSpeed * Vector3.forward);
    }
}

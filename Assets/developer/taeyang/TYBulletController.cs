using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TYBulletController : MonoBehaviour
{   
    [SerializeField] private int damage;    
    [SerializeField] private float bulletSpeed;
    
    void Start()
    {
        Destroy(gameObject, 2f);
    }
    
    void Update()
    {
        transform.Translate(Time.deltaTime * bulletSpeed * Vector3.forward);
    }
}

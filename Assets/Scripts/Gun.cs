using Assets.Scripts;
using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour,IGun
{

    public GameObject bullet,muzzle;
    public Transform position1, position2;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Reload()
    {
    }

    public void Shoot()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            bullet.GetComponent<BulletEnemy>().currentFoward = transform.forward;

        }
        Instantiate(bullet, position1.position, Quaternion.identity);
        Instantiate(bullet, position2.position, Quaternion.identity);
        Instantiate(muzzle, position1.position, Quaternion.identity);
        Instantiate(muzzle, position2.position, Quaternion.identity);
        
    }
}

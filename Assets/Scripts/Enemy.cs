using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public GameObject hurtEffect, deathEffect;
    RipplePostProcessor ripple;
    GameObject player;
    public float minDistance = 5;
    Rigidbody rb;
    Quaternion defaultRot;
    Vector3 originalPos;
    public float Speed = 5f;
    public float minRetreatDistance = 8;
    public float stopDistance = 9;
    public float distanceFromPlayer;
    public GameObject[] AIS;
    public float minDistanceBetweenAI = 5f;
    public float factor = 5f;
    public float force = 5;
    public float awayFactor = -30;
    public float delay = 5;
    public Vector3 destroyVector;
    public bool ignore = false;
    public float MinX = 15.3f;
    public float Health;
    public Quaternion defaltRot;
    public float defaultY;
    public float apartForce = 10;
    public float health;
    [Range(0,1)]
    public float decreaseFactor;
    Quaternion rotation;
    bool isPlayerGrounded = false;
    public Gun gun;
    public float maxShootTime = 1.5f;
    public float currentShootTime;
    // Start is called before the first frame update
    void Start()
    {
        health = Random.Range(5, 15);
        defaultRot = transform.rotation;
        originalPos = transform.position;
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        ripple = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<RipplePostProcessor>();
        currentShootTime = maxShootTime;
    }
    // Update is called once per frame
    void Update()
    {
        checkDeslam();
        ResetPosition();
        move();
        facePlayer();
        shoot();
        
    }

    private void shoot()
    {
        if (currentShootTime <= 0)
        {
            currentShootTime = maxShootTime;
            gun.Shoot();
        }
        else
        {
            currentShootTime -= Time.deltaTime;
        }
    }

    private void facePlayer()
    {
        transform.LookAt(player.transform.position);
    }
    private void move()
    {
        if (!ignore)
        {
            distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceFromPlayer > stopDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Speed * Time.deltaTime);
            }
            else if (distanceFromPlayer < stopDistance && distanceFromPlayer > minRetreatDistance)
            {
                transform.position = transform.position;
            }
            else if (distanceFromPlayer < minRetreatDistance && distanceFromPlayer < minRetreatDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -Speed * Time.deltaTime);
            }
            for (int i = 0; i < AIS.Length; i++)
            {
                if (AIS[i] == null)
                {
                    continue;
                }
                Transform temp = AIS[i].transform;
                float distance = Vector3.Distance(transform.position, temp.position);
                if (distance < minDistanceBetweenAI && temp != transform)
                {
                    Vector3 target = temp.position - transform.position;
                    target = target.normalized;
                    temp.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(-target.x, target.y, -target.z));
                }
            }
        }
    }
    private void ResetPosition()
    {
        if (transform.position.y < 0)
        {
            transform.position = originalPos;
            transform.rotation = defaultRot;
        }
    }
    void checkDeslam()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        var scritp = player.GetComponent<Player>();
        var force = scritp.deslamjumpForce;
        var didSlam = scritp.jumped;
        minDistance = scritp.slamDistance;
        if (didSlam) {
            if (distance >= minDistance)
            {
                rb.AddForce(new Vector3(-player.transform.position.x, transform.position.y, -player.transform.position.z) * force );
            }
        }
        Debug.Log("Distance: " + distance);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            health -= Time.deltaTime + decreaseFactor;
            if (health <= 0)
            {
                Instantiate(deathEffect, transform.position, Quaternion.identity);
                DestroyEnemy();
            }
            else
            {
                Instantiate(hurtEffect, transform.position, Quaternion.identity);
            }
        }
        ripple.Ripple();
    }
}

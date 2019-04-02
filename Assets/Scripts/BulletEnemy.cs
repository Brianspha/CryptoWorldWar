using Assets.Scripts.Interfaces;
using EZCameraShake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class BulletEnemy: MonoBehaviour,IBullet
    {
        public float force = 500;
        Rigidbody rb;
        Enemy player;
        CameraShaker shaker;
        public float Speed { get; private set; }
        public float magn = 1000, rough = 500, fadeIn = .08f, fadeOut = .04f;
        bool shookAlready = false;
        public Vector3 currentFoward;
        private void Start()
        {
            Speed = 30;
            Debug.Log("Parent: " + gameObject.transform.parent.gameObject.tag);
            currentFoward = GameObject.FindGameObjectWithTag("Player").transform.forward;
            shaker = Camera.main.GetComponent<CameraShaker>();
            //player = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
            rb = GetComponent<Rigidbody>();
        }
        private void Update()
        {
            transform.position += currentFoward * Speed * Time.deltaTime;
        }
        //void Go()
        //{
        //    rb.AddForce(player.currentFoward * force);
        //}

    }
}

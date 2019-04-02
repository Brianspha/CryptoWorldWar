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
    public class Bullet:MonoBehaviour,IBullet
    {
        public float force = 500;
        Rigidbody rb;
        Player player;
        CameraShaker shaker;
        public float Speed { get; private set; }
        public float magn = 1000, rough = 500, fadeIn = .08f, fadeOut = .04f;
        bool shookAlready = false;
        private void Start()
        {
            Speed = 30;
            shaker = Camera.main.GetComponent<CameraShaker>();
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            rb = GetComponent<Rigidbody>();
        }
        private void Update()
        {
            if (!shookAlready)
            {
                shaker.ShakeOnce(magn, rough, fadeIn, fadeOut);
            }
            //shaker.ShakeOnce(magn, rough, fadeIn, fadeOut);
            transform.position += player.currentFoward * Speed * Time.deltaTime;
        }
        void Go()
        {
            rb.AddForce(player.currentFoward * force);
        }

    }
}

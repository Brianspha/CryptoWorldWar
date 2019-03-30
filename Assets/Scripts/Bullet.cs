using Assets.Scripts.Interfaces;
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
        public float force = 1500;
        Rigidbody rb;
        Player player;
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            rb = GetComponent<Rigidbody>();
            Go();
        }
        private void Update()
        {
        }
        void Go()
        {
            rb.AddForce(player.currentFoward * force,ForceMode.Acceleration);
        }
    }
}

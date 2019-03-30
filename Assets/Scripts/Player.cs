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
    public class Player:MonoBehaviour,IPlayer
    {
        public float Speed = 10f;
        Vector3 moveVector;
        public float moveIncrementor = -3;
        public float minY = 1.03f;
        public bool grounded;
        public Rigidbody rb;
        public float jumpForce = 10;
        public float deslamjumpForce = 20;
        Quaternion defaultRot;
        CameraShaker shaker;
        public float magn = 1000, rough = 500, fadeIn = 1f, fadeOut = 2f;
        public GameObject dashEffect;
        public bool jumped = false;
        private void Start()
        {
            shaker = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShaker>();
            defaultRot = transform.rotation;
            rb = GetComponent<Rigidbody>();
        }
        public void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.A))//@Dev move left
            {
              moveVector = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x+Vector3.left.x, transform.position.y, transform.position.z),Speed * Time.deltaTime);
              transform.position = moveVector;
            }
            if (Input.GetKey(KeyCode.D))//@Dev move right
            {
                moveVector = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + Vector3.right.x, transform.position.y, transform.position.z), Speed * Time.deltaTime);
                transform.position = moveVector;
            }
            if (Input.GetKey(KeyCode.S))//@Dev move down
            {
                moveVector = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x , transform.position.y, transform.position.z-Vector3.forward.z), Speed * Time.deltaTime);
                transform.position = moveVector;
            }
            if (Input.GetKey(KeyCode.W))//@Dev move up
            {
                moveVector = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + Vector3.forward.z), Speed * Time.deltaTime);
                transform.position = moveVector;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                  Slam();
                  Debug.Log("Called Slam");
            } 
            if(!grounded && Input.GetKeyDown(KeyCode.Mouse2)){
                deSlam();
                jumped = true;
            }
        }

        public void deSlam()
        {
            rb.AddForce(new Vector3(0, -deslamjumpForce, 0), ForceMode.Impulse);
            shaker.ShakeOnce(magn, rough, fadeIn, fadeOut);
        }
        public void Slam()
        {
            if (grounded)
            {
                rb.AddForce(new Vector3(0,jumpForce,0),ForceMode.Impulse);
                grounded = false;
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Called");
            if (collision.gameObject.CompareTag("Floor"))
            {
                grounded = true;
                Debug.Log("Im touching the ground");
                if (jumped)
                {
                    Instantiate(dashEffect, transform.position, Quaternion.identity);
                    jumped = false;
                }
                transform.rotation = defaultRot;
            }
        }
    }
}

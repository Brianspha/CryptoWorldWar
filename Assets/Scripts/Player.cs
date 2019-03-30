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
        public float distance = 1000;
        public Gun playerGun;
        public Vector3 currentFoward;
        Vector3 originalPos;
        private void Start()
        {
            originalPos = transform.position;
            shaker = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShaker>();
            defaultRot = transform.rotation;
            rb = GetComponent<Rigidbody>();
        }
        private void Update()
        {
          faceMouse();
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
            if (Input.GetMouseButtonDown(0) && grounded)
            {
                  playerGun.Shoot();
            }
            if (!grounded && Input.GetKeyDown(KeyCode.Mouse2)){
                deSlam();
                jumped = true;
            }
            if (transform.position.y < 0)
            {
                ResetPlayer();
            }
            currentFoward = transform.forward;
        }

        private void faceMouse()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, distance))
            {
                transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z),Vector3.up);
            }

                //Vector3 direction = Camera.main.WorldToViewportPoint(Input.mousePosition) - transform.position;
                //var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                //Quaternion rotation = Quaternion.AngleAxis(-angle, Vector3.up);
                //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed);
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
        public void ResetPlayer()
        {
            transform.position = originalPos;
        }
    }
}

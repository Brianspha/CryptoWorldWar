using EZCameraShake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour, IPlayer
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
        public float slamDistance = 5;
        public Text collected;
        public string Address;
        CollectableManager Manager;
        public List<int> CollectiblesCollected;
        public GameObject rocket;
        bool transfered;
        private void Start()
        {
            CollectiblesCollected = new List<int>();
            originalPos = transform.position;
            shaker = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShaker>();
            defaultRot = transform.rotation;
            rb = GetComponent<Rigidbody>();
            Manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CollectableManager>();
            StartCoroutine(Manager.RegisterPlayer(Address));
        }
        private void Update()
        {
            faceMouse();
            if (Manager.deadSoFar == Manager.maxEnemy && !transfered)
            {
                for (int i = 0; i < CollectiblesCollected.Count; i++) {
                    StartCoroutine(Manager.TransferCollectible(i, Address));
                }
                transfered = true;
            }
        }
        public void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (Input.GetKey(KeyCode.A))//@Dev move left
            {
                moveVector = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + Vector3.left.x, transform.position.y, transform.position.z), Speed * Time.deltaTime);
                transform.position = moveVector;
            }
            if (Input.GetKey(KeyCode.D))//@Dev move right
            {
                moveVector = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + Vector3.right.x, transform.position.y, transform.position.z), Speed * Time.deltaTime);
                transform.position = moveVector;
            }
            if (Input.GetKey(KeyCode.S))//@Dev move down
            {
                moveVector = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z - Vector3.forward.z), Speed * Time.deltaTime);
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
                //Debug.Log("Called Slam");
            }
            if (Input.GetMouseButtonDown(0) && grounded)
            {
                playerGun.Shoot();
                transform.position = new Vector3(transform.position.x, minY, transform.position.z);
            }
            if(Input.GetMouseButtonDown(1) && grounded)
            {
                playerGun.shootRocket();
                transform.position = new Vector3(transform.position.x, minY, transform.position.z);

            }
            if (!grounded && Input.GetKeyDown(KeyCode.Mouse2))
            {
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
                transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z), Vector3.up);
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
                rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
                grounded = false;
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            ////Debug.Log("Called");
            if (collision.gameObject.CompareTag("Floor"))
            {
                grounded = true;
              //  //Debug.Log("Im touching the ground");
                if (jumped)
                {
                    Instantiate(dashEffect, transform.position, Quaternion.identity);
                    jumped = false;
                }
                transform.rotation = defaultRot;
            }
            switch (collision.gameObject.tag)
            {
                case "PowerUpGun":
                    //Debug.Log("CollectedGun");
                    collected.text = "Gun";
                    Destroy(collision.gameObject);
                    break;
                case "BulletPowerUp":
                    //Debug.Log("Collected new Bullet");
                    collected.text = "Bullet";
                    Destroy(collision.gameObject);
                    break;
                case "PowerUpArmor":
                    //Debug.Log("Collected new Armor");
                    collected.text = "Armor";
                    Destroy(collision.gameObject);
                    break;
                case "PowerUpStamina":
                    //Debug.Log("Collected new Stamina");
                    collected.text = "Stamina";
                    Destroy(collision.gameObject);
                    break;
                default:
                    //Debug.Log("Found: " + collision.gameObject.tag);
                    break;
            }
        }
        public void ResetPlayer()
        {
            transform.position = originalPos;
        }
    }
}
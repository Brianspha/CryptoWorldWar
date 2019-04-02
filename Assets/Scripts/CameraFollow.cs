using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float smoothSpeed;
    public Transform offet;
    public float offSetFromTarget;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
                target = GameObject.FindGameObjectWithTag("Player").transform;

    }
    void FixedUpdate()
    {
        Vector3 desiredPos = new Vector3(transform.position.x, transform.position.y, target.position.z + offSetFromTarget);
       // Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * smoothSpeed);
        transform.position = desiredPos;
    }
}

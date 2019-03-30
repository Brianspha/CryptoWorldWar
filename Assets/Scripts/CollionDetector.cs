using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollionDetector : MonoBehaviour
{
    // Start is called before the first frame update
    public string toDetect;
    public GameObject toSpawn;

    // Update is called once per frame
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(toDetect))
        {
            Instantiate(toSpawn, transform.position, Quaternion.identity);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour {

	public int openingDirection;
	// 1 --> need bottom door
	// 2 --> need top door
	// 3 --> need left door
	// 4 --> need right door


	private RoomTemplates templates;
	private int rand;
	public bool spawned = false;

	public float waitTime = 8f;
    public bool close = false;
	void Start(){
		Destroy(gameObject, waitTime);
		templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Spawn();
	}


    void Spawn()
    {
        if (close)
        {
            Instantiate(templates.closedRoom, transform.position, templates.closedRoom.transform.rotation);
            spawned = true;
            Destroy(gameObject);
        }
        if (spawned == false)
        {
            if (openingDirection == 1)
            {
                // Need to spawn a room with a BOTTOM door.
                rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
            }
            else if (openingDirection == 2)
            {
                // Need to spawn a room with a TOP door.
                rand = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
            }
            else if (openingDirection == 3)
            {
                // Need to spawn a room with a LEFT door.
                rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
            }
            else if (openingDirection == 4)
            {
                // Need to spawn a room with a RIGHT door.
                rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
            }
            spawned = true;
        }
    }
  
	void OnTriggerEnter(Collider other){
        //if (other.gameObject.CompareTag("SpawnPoint"))
        //{
        //    if (openingDirection == 0)
        //    {
        //        Destroy(other.gameObject);
        //    }
        //    else if (other.gameObject.GetComponent<RoomSpawner>().openingDirection == 0)
        //    {
        //        Destroy(gameObject);
        //    }
        //    else if(other.gameObject.GetComponent<RoomSpawner>().openingDirection !=0 && openingDirection != 0)
        //    {
        //        other.gameObject.GetComponent<RoomSpawner>().spawned = true;
        //        spawned = true;

        //    }
        //}
        //if (other.CompareTag("SpawnPoint"))
        //{
        //    // Debug.Log("Yay called: " + templates.closedRoom);
        //    //if (templates == null) return;
        //    if (other.gameObject.GetComponent<RoomSpawner>().spawned == false && spawned == false)
        //    {
        //        Debug.Log("Current Object: " + templates.closedRoom);
        //        Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
        //        Destroy(gameObject);
        //        Debug.Log("Called");
        //    }
        //    spawned = true;
        //}
    }
}

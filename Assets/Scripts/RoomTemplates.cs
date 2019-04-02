using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class RoomTemplates : MonoBehaviour {

	public GameObject[] bottomRooms;
	public GameObject[] topRooms;
	public GameObject[] leftRooms;
	public GameObject[] rightRooms;

	public GameObject closedRoom;

	public List<GameObject> rooms;

	public float waitTime;
	private bool spawnedBoss;
	public GameObject boss;
    private void Start()
    {
        bottomRooms = copyArray(topRooms.ToList(), bottomRooms.ToList());
        bottomRooms = copyArray(leftRooms.ToList(), bottomRooms.ToList());
        bottomRooms = copyArray(rightRooms.ToList(), bottomRooms.ToList());


        //topRooms = copyArray(bottomRooms.ToList(), topRooms.ToList());
        //topRooms = copyArray(leftRooms.ToList(), topRooms.ToList());
        //topRooms = copyArray(rightRooms.ToList(), topRooms.ToList());

        leftRooms = copyArray(bottomRooms.ToList(), leftRooms.ToList());
        leftRooms = copyArray(topRooms.ToList(), leftRooms.ToList());
        leftRooms = copyArray(rightRooms.ToList(), leftRooms.ToList());


        //rightRooms = copyArray(bottomRooms.ToList(), rightRooms.ToList());
        //rightRooms = copyArray(topRooms.ToList(), rightRooms.ToList());
        //rightRooms = copyArray(leftRooms.ToList(), rightRooms.ToList());



    }
    GameObject[] copyArray(List<GameObject> from, List<GameObject>to)
    {
        for(int i=0; i < from.Count; i++)
        {
            to.Add(from[i]);
        }
        return from.ToArray();
    }
    void Update(){

		if(waitTime <= 0 && !spawnedBoss){
			for (int i = 0; i < rooms.Count; i++) {
				if(i == rooms.Count-1){
					Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
					spawnedBoss = true;
				}
			}
		} else {
			waitTime -= Time.deltaTime;
		}

	}
}

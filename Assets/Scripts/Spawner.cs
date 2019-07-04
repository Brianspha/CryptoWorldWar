using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform right, Top;
    public GameObject enemy;
    public int maxEnemy = 5;
    public int maxCollectible = 4;
    public float minY = .55f,minYCollectibles= 0.987f,minDistanceApart=25;
    public List<GameObject> environmentObjects;
    CollectableManager CollectableManager;
    void Start()
    {
        spawn();
        spawnEnvironment();
    }

    private void spawnEnvironment()
    {
        
    }

    private void spawn()
    {
        var spawned = new List<Vector3>();
        for(int i=0; i < maxEnemy; )
        {
            var pos = new Vector3(Random.Range(-right.position.x, right.position.x), minY, Random.Range(-Top.position.z, Top.position.z));
            if (checkMinDistanceApart(pos,spawned))
            {
                enemy.GetComponent<Enemy>().attachedCollectable=
                Instantiate(enemy,pos, Quaternion.identity);
                spawned.Add(pos);
                i++;
            }
        }
    }

    public bool checkMinDistanceApart(Vector3 pos,List<Vector3> spawned)
    {
        if (spawned.Count == 0)
        {
            return true;
        }
        bool ok = false;
        var apart = new List<Vector3>();
        for (int i =0; i < spawned.Count; i++)
        {
            var dist = Vector3.Distance(pos, spawned[i]) ;
            if (dist >= minDistanceApart)
            {
                apart.Add(spawned[i]);
            }
        }
        if(apart.Count < spawned.Count)
        {
            for(int i =0;i < spawned.Count; i++)
            {
                if (!apart.Contains(spawned[i]))
                {
                    spawned.RemoveAt(i);
                }
            }
        }
        return ok;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

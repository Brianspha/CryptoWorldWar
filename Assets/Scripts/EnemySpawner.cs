using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform right, Top;
    public GameObject enemy;
    public int maxEnemy = 5;
    public int maxCollectible = 4;
    public float minY = .55f,minYCollectibles= 0.987f,minDistanceApart=25;
    public List<GameObject> collectibles;
    void Start()
    {
        spawn();
        spawnCollectibles();
    }

    private void spawnCollectibles()
    {
        var spawned = new List<Vector3>();
        for (int i = 0; i < maxCollectible;)
        {
            var pos = new Vector3(Random.Range(-right.position.x, right.position.x), minYCollectibles, Random.Range(-Top.position.z, Top.position.z));
            if (checkMinDistanceApart(pos, spawned))
            {
                Instantiate(collectibles[Random.Range(0, collectibles.Count)], pos, Quaternion.identity);
                spawned.Add(pos);
                i++;
            }
        }
    }

    private void spawn()
    {
        var spawned = new List<Vector3>();
        for(int i=0; i < maxEnemy; )
        {
            var pos = new Vector3(Random.Range(-right.position.x, right.position.x), minY, Random.Range(-Top.position.z, Top.position.z));
            if (checkMinDistanceApart(pos,spawned))
            {
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
        for (int i =0; i < spawned.Count; i++)
        {
            var dist = Vector3.Distance(pos, spawned[i]) ;
            Debug.Log(dist);
            if (dist >= minDistanceApart)
            {
                ok = true;
            }
        }
        return ok;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

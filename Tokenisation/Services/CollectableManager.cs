using Assets.SmartContracts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{

    public GameObject CollectableTemplate;
    List<Collectable> collectables;
    public List<Collectable> Collectables
    {
        get
        {
            return collectables;
        }
        set
        {
            collectables = value;
        }
    }
    // Start is called before the first frame update
    public Transform Right, Top;
    public GameObject enemy;
    public int maxEnemy = 5;
    public int maxCollectible = 4;
    public float minY = .55f, minYCollectibles = 0.987f, minDistanceApart = 25;
    public List<Collectable> collectibles;
    public List<GameObject> environmentObjects;
    void Start()
    {
    }
    public CollectableManager(GameObject template)
    {
        CollectableTemplate = template;
        Right = GameObject.FindGameObjectWithTag("Right").transform; 
        Top = GameObject.FindGameObjectWithTag("Top").transform;
        Collectables = new List<Collectable>();
        spawnCollectibles();
    }
    private void spawnCollectibles()
    {
        var spawned = new List<Vector3>();

        for (int i = 0; i < maxCollectible;)
        {
            var pos = new Vector3(Random.Range(-Right.position.x, Right.position.x), minYCollectibles, Random.Range(-Top.position.z, Top.position.z));
            if (checkMinDistanceApart(pos, spawned))
            {
                Collectable temp = new Collectable { CollectableObject = null, Tag = "PowerUpArmor" };//@notice this will change in the future for now we are 
               GameObject spawnedCollectable=Instantiate(CollectableTemplate, pos, Quaternion.identity);
                Collectables.Add();
                spawned.Add(pos);
                i++;
            }
        }
    }


    private bool checkMinDistanceApart(Vector3 pos, List<Vector3> spawned)
    {
        if (spawned.Count == 0)
        {
            return true;
        }
        bool ok = false;
        var apart = new List<Vector3>();
        for (int i = 0; i < spawned.Count; i++)
        {
            var dist = Vector3.Distance(pos, spawned[i]);
            Debug.Log(dist);
            if (dist >= minDistanceApart)
            {
                apart.Add(spawned[i]);
            }
        }
        if (apart.Count < spawned.Count)
        {
            for (int i = 0; i < spawned.Count; i++)
            {
                if (!apart.Contains(spawned[i]))
                {
                    spawned.RemoveAt(i);
                }
            }
        }
        return ok;
    }
}

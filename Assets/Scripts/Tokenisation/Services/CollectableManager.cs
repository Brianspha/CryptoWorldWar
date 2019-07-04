using Assets.SmartContracts.Models;
using System.Collections.Generic;
using UnityEngine;
using Nethereum.JsonRpc.UnityClient;
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
    public GameObject enemy;
    public int maxEnemy = 5;
    public int maxCollectible = 4;
    public float minY = .55f, minYCollectibles = 0.987f, minDistanceApart = 25;
    public List<GameObject> environmentObjects;
    CryptoWorldWarService CryptoWorldWarService;
    void Start()
    {
        CryptoWorldWarService = new CryptoWorldWarService();
        StartCoroutine(CryptoWorldWarService.RegisterPlayer());
        Collectables = new List<Collectable>();
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

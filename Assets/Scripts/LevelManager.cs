using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameObject [,] Grid;
    public GameObject block;
    public int sizeX = 5;
    public int sizeY = 5;
    public float incrementor = 10;
    // Start is called before the first frame update
    void Start()
    {
        Grid = new GameObject[sizeX,sizeY];
        block.transform.position = Vector3.zero;
        SetUpGrid();
       
    }



    // Update is called once per frame
    void Update()
    {
        
    }
    private void SetUpGrid()
    {
        for(int x=0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                block.transform.position = new Vector3(block.transform.position.x + incrementor, block.transform.position.y, block.transform.position.z);
                Grid[x, y] = block;
                SpawnBlock(block);
            }
            block.transform.position = new Vector3(0, block.transform.position.y, block.transform.position.z + incrementor);
        }
    }

    private void SpawnBlock(GameObject block)
    {
        Instantiate(block, block.transform.position, transform.rotation);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class cudeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;

    // Update is called once per frame
    public void SpawnObj()
    {
        
        
            Vector3[] li = {new Vector3(0,5,-10),new Vector3(0,5,11)};

            Vector3 randomPos = li[Random.Range(0,2)];
            Instantiate(cubePrefab,randomPos,Quaternion.identity);
        
    }
}

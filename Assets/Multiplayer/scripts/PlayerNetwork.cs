using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
public class PlayerNetwork : NetworkBehaviour // NetworkBehaviour = mono mais avec des feature multi en plus
{
    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1);
    bool isHere = false;
    [SerializeField] private Transform spawnedObjectPrefab;
    private void Start() {

        var objs = FindObjectsOfType<Item.Item>();
        if (IsLocalPlayer)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                isHere = objs[i].IsSpawned;
                objs[i].gameObject.SetActive(isHere);
            }
            
        }    
    } 

    private void Update() {
        if (!IsOwner) return;

        Vector3[] li = {new Vector3(-10,5,-0),new Vector3(10,5,0)};

        Vector3 randomPos = li[Random.Range(0,2)];
        spawnedObjectPrefab.SetPositionAndRotation(randomPos,Quaternion.identity);
        if (Input.GetKeyDown(KeyCode.T)){
            Transform spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
        }

    }
}

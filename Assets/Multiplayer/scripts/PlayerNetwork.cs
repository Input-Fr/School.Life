using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour // NetworkBehaviour = mono mais avec des feature multi en plus
{
    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1);

    [SerializeField] private Transform spawnedObjectPrefab;
   
    private void Update() {
        if (!IsOwner || !IsHost) return ;
        Vector3[] li = {new Vector3(-10,5,-0),new Vector3(10,5,0)};

        Vector3 randomPos = li[Random.Range(0,2)];
        spawnedObjectPrefab.SetPositionAndRotation(randomPos,Quaternion.identity);
        if (Input.GetKeyDown(KeyCode.T)){
            Transform spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
        }

    }
}

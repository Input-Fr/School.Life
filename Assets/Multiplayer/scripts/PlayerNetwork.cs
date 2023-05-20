using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using User;
using Unity.Netcode;
using TMPro;
using System;
using Door;
using StarterAssets;
public class PlayerNetwork : NetworkBehaviour // NetworkBehaviour = mono mais avec des feature multi en plus
{
    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1);

    private BatState batstate;
    bool isHere = false;
    public bool localPlayer;
    [SerializeField] private Transform spawnedObjectPrefab;
    [SerializeField] private GameObject professorPrefab;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject[] itemsPrefabs;
    [SerializeField] private Behaviour[] componentsToDisable;
    [SerializeField] private GameObject interfaceCanvas;
    [SerializeField] private GameObject filter;
    [SerializeField] private GameObject doorPrefab;
    NavMeshSurface _surface;
    private void Start() {
        localPlayer = IsLocalPlayer;
        var objs = FindObjectsOfType<Item.Item>();
        if (IsLocalPlayer)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            for (int i = 0; i < objs.Length; i++)
            {
                isHere = objs[i].IsSpawned;
                objs[i].gameObject.SetActive(isHere);
            }
            
        }    
        else
        {
            batstate.ChangeStateBatServerRpc(batstate.hasBatInHand.Value);
            foreach (Behaviour component in componentsToDisable)
            {
                component.enabled = false;
            }

            interfaceCanvas.SetActive(false);
            cam.SetActive(false);
        }
    }

    private void Awake() {
        batstate = GetComponent<BatState>();
        _surface = GameObject.FindGameObjectWithTag("Platform").GetComponent<NavMeshSurface>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsLocalPlayer || !IsHost) return;
    
        Vector3 pos = new Vector3(0,0,20);
        foreach (GameObject item in itemsPrefabs)
        {
            for (int i = 0; i < 2; i++)
            {
                pos += new Vector3(0, 0, 2);
                InstantiateItem(item, pos);
            }
            pos += new Vector3(0, 0, 2);
        }


        InstantiateProfessorServerRpc(-78, 0.49f, -14.2f);
        InstantiateProfessorServerRpc(-76, 0.49f, -12.62f);

        InstantiateDoorServerRpc(0,0,15);

        _surface.BuildNavMesh();
    }

    [ServerRpc(RequireOwnership = false)]
    private void InstantiateProfessorServerRpc(float x, float y, float z)
    {
        Debug.Log("Instantiate professor");
        GameObject instantiatedProfessor = Instantiate(professorPrefab, new Vector3(x, y, z), Quaternion.identity);
        instantiatedProfessor.GetComponent<NetworkObject>().Spawn(true);
    }

    private void Update() {
        if (!IsOwner) return;

        Vector3[] li = {new Vector3(-10,5,-0),new Vector3(10,5,0)};

        Vector3 randomPos = li[UnityEngine.Random.Range(0,2)];
        spawnedObjectPrefab.SetPositionAndRotation(randomPos,Quaternion.identity);
        if (Input.GetKeyDown(KeyCode.T)){
            Transform spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
        }
    }

    public void InstantiateItem(GameObject item, Vector3 pos)
        {
    
            for (int i = 0; i < itemsPrefabs.Length; i++)
            {
                Debug.Log(item.name, itemsPrefabs[i]);
                if (itemsPrefabs[i] != item) continue;
                InstantiateItemServerRpc(i, pos.x, pos.y, pos.z);
                return;
            }

            throw new Exception();
        }

        [ServerRpc(RequireOwnership = false)]
        private void InstantiateItemServerRpc(int index, float x, float y, float z)
        {
            GameObject item = itemsPrefabs[index];
            GameObject instantiatedItem = Instantiate(item, new Vector3(x, y, z), Quaternion.identity);
            instantiatedItem.GetComponent<NetworkObject>().Spawn(true);
        }

        [ServerRpc(RequireOwnership = false)]
        private void InstantiateDoorServerRpc(float x, float y, float z)
        {
            GameObject instantiatedDoor = Instantiate(doorPrefab, new Vector3(x, y, z), Quaternion.identity);
            instantiatedDoor.GetComponent<NetworkObject>().Spawn(true);
            MyDoorController door = instantiatedDoor.GetComponentInChildren<MyDoorController>();
            door.ChangeVariableServerRpc(door.gameObject.tag == "Locked");
        }

        [ServerRpc(RequireOwnership = false)]
        public void ChangeCanMoveCameraServerRpc(int id)
        {
            ChangeCanMoveCameraClientRpc(id);
        }

        [ClientRpc]
        private void ChangeCanMoveCameraClientRpc(int id)
        {
            if ((int)NetworkObjectId == id)
            {
                foreach (Transform child in transform)
                {
                    if (TryGetComponent(out ThirdPersonController vThirdPersonController))
                    {
                        StartCoroutine(ChangeCanMoveCamera(vThirdPersonController));
                        return;
                    }
                }

                throw new Exception();
            }
        }
        
        private IEnumerator ChangeCanMoveCamera(ThirdPersonController vThirdPersonController)
        {
            vThirdPersonController.canOnlyWalk = true;
            filter.SetActive(true);

            yield return new WaitForSeconds(5f);
            
            filter.SetActive(false);
            vThirdPersonController.canOnlyWalk = false;
        }
}


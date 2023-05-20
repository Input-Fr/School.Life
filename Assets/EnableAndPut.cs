using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class EnableAndPut : MonoBehaviour
{
    public GameObject toFollow;
    public GameObject camInG;
    public GameObject camMenu;
    public GameObject copain;
    
    private void OnClientConnectedCallback(ulong u) 
    {
        var Players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in Players)
        {
            if (player.GetComponent<PlayerInput>() && !player.GetComponent<NetworkObject>().IsLocalPlayer)
            {
                player.GetComponent<PlayerInput>().enabled = false;
            }
        }
        foreach (var player in Players)
        {
            if (player.GetComponent<PlayerInput>() && player.GetComponent<NetworkObject>().IsLocalPlayer)
            {
                player.GetComponent<PlayerInput>().enabled = false;
                player.GetComponent<PlayerInput>().enabled = true;
            }
        }
        var ObjstoFollow = GameObject.FindGameObjectsWithTag("CinemachineTarget");
        toFollow = ObjstoFollow[ObjstoFollow.Length-1];
        var mCopain = copain.GetComponent<CinemachineVirtualCamera>();
        mCopain.Follow = toFollow.transform;
        camInG.AddComponent<Item.ItemsInteraction>();
        camInG.AddComponent<Door.DoorsInteraction>();
        camInG.AddComponent<ButtonD.ButtonD>();
        
    }
    private void Start() {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

    }

    private void OnDisable() {
        //NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
    }
    

}

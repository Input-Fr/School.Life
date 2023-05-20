using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using disableIfNotMine;
using Door;
using Item;
using ButtonD;
public class enableWhenConnected : MonoBehaviour
{


    private void OnClientConnectedCallback(ulong u) 
    {
        this.GetComponent<ButtonD.ButtonD>().enabled = true;
        this.GetComponent<Item.ItemsInteraction>().enabled = true;
        this.GetComponent<Door.DoorsInteraction>().enabled = true;
        this.GetComponent<disableIfNotMine.disableIfNotMine>().enabled = true;
    }
    private void Start() {
        this.GetComponent<ButtonD.ButtonD>().enabled = false;
        this.GetComponent<Item.ItemsInteraction>().enabled = false;
        this.GetComponent<Door.DoorsInteraction>().enabled = false;
        this.GetComponent<disableIfNotMine.disableIfNotMine>().enabled = false;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

    }

    private void OnDisable() {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
    }
}

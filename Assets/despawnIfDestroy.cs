using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class despawnIfDestroy : NetworkBehaviour
{
    NetworkVariable<bool> isHere = new NetworkVariable<bool>(true);

    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            isHere.Value = gameObject.GetComponent<NetworkObject>().IsSpawned;
        }
        if (IsLocalPlayer)
        {
            this.gameObject.SetActive(isHere.Value);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
public class disableIfNotMine : NetworkBehaviour
{
    // Start is called before the first frame update
    

    void Start()
    {
        

        if(!NetworkObject.IsOwner)
        {
            gameObject.SetActive(false);
        }        
    }

}

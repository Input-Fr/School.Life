using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class ServerHosted : NetworkBehaviour
{
    [SerializeField] Transform texte;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (texte.gameObject.activeInHierarchy)
            {
                texte.gameObject.SetActive(false);
            }
            else{
                texte.gameObject.SetActive(true);
            }
        }
    }
}

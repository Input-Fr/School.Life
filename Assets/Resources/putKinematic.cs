using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class putKinematic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<Rigidbody>().isKinematic) return;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
}

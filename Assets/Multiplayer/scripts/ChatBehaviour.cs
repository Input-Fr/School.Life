using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using System;
public class ChatBehaviour : NetworkBehaviour
{
    [SerializeField] private GameObject chatUI;
    [SerializeField] private TMP_Text chatText;
    [SerializeField] private TMP_InputField inputField;

    private static event Action<string> OnMessage;
    
    

    // Update is called once per frame
    void Update()
    {
        
    }
}

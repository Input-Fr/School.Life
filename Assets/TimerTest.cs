using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
public class TimerTest : NetworkBehaviour
{

    [SerializeField] private TextMeshProUGUI textGeneric;
    [SerializeField] private TextMeshProUGUI textTime;
    [SerializeField] private TextMeshProUGUI textEvent;

    private NetworkVariable<float> timeInSecMCQ = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<float> timeInSecPause = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> isInMCQ = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    // Start is called before the first frame update
    private void Start() {
        textGeneric.text = "Remaining time before ";   
        textEvent.text = "next Test : ";
        if (!IsOwner) 
        {
            return;
        }
        
        timeInSecMCQ = new NetworkVariable<float>(480, NetworkVariableReadPermission.Everyone);
        timeInSecPause = new NetworkVariable<float>(60, NetworkVariableReadPermission.Everyone);
        isInMCQ = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone);
        

    }

  

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(OwnerClientId + " : " + timeInSecMCQ.Value);
        if (!IsOwner)
        {
            if (isInMCQ.Value)
            {
                textTime.text = $"{(int)(timeInSecMCQ.Value/60)}' {((int)(timeInSecMCQ.Value) % 60)}''";
                textEvent.text = "next Test : ";
            }
            else
            {

                textEvent.text = "next MCQ training : ";
                textTime.text = $"{(int)(timeInSecPause.Value/60)}' {((int)(timeInSecPause.Value) % 60)}''";
            }
            
            return;
        } 

        if (timeInSecMCQ.Value > 0)
        {
            if (isInMCQ.Value)
            {
                timeInSecMCQ.Value -= Time.deltaTime;
                textTime.text = $"{(int)(timeInSecMCQ.Value/60)}' {((int)(timeInSecMCQ.Value) % 60)}''";
            }
            else
            {
                isInMCQ.Value = true;
                textEvent.text = "next Test : ";
            }

        }
        else
        {
            isInMCQ.Value = false;
        }

        if (!isInMCQ.Value)
        {
            if (timeInSecPause.Value > 0)
            {
                textEvent.text = "next class : ";
                timeInSecPause.Value -= Time.deltaTime;
                textTime.text = $"{(int)(timeInSecPause.Value/60)}' {((int)(timeInSecPause.Value) % 60)}''";
            }
            else
            {
                timeInSecMCQ.Value = 480;
                timeInSecPause.Value = 60;
            }
        }
    }
}

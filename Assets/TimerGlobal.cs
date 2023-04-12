using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
public class TimerGlobal : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI textGeneric;
    [SerializeField] private TextMeshProUGUI textTime;
    [SerializeField] private TextMeshProUGUI textEvent;
    bool isNotInPause = false;
    float timeInSecMCQ = 480;
    float timeInSecPause = 60;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    

    [ClientRpc]
    void updateClClientRpc(int min,int sec, bool isInPauseorNot)
    {
        textTime.text = $"{min}' {sec}''";
        if (isInPauseorNot)
        {
            textEvent.text = "next test : ";
        }
        else
        {
            textEvent.text = "next class : ";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        if (timeInSecMCQ > 0)
        {
            if (isNotInPause)
            {
                timeInSecMCQ -= Time.deltaTime;
                updateClClientRpc(((int)timeInSecMCQ)/60,((int)timeInSecMCQ)%60, isNotInPause);
            }
            else
            {
                isNotInPause = true;
                updateClClientRpc(((int)timeInSecMCQ)/60,((int)timeInSecMCQ)%60, isNotInPause);
            }

        }
        else
        {
            isNotInPause = false;
        }

        if (!isNotInPause)
        {
            if (timeInSecPause > 0)
            {
                timeInSecPause -= Time.deltaTime;
                updateClClientRpc((int)timeInSecPause/60,((int)timeInSecPause)%60, isNotInPause);
            }
            else
            {
                timeInSecMCQ = 480;
                timeInSecPause = 60;
            }
        }
        
    }
}

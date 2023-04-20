using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class hideShowInfo : MonoBehaviour
{

    public GameObject infoToModify;
    public TextMeshProUGUI infoTipToModify;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (infoToModify.activeInHierarchy)
            {
                infoToModify.SetActive(false);
                infoTipToModify.text = "Press H to show code";
            }
            else
            {
                infoToModify.SetActive(true);
                infoTipToModify.text = "Press H to hide code";
            }
        }
    }
}

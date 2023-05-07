using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Switch : MonoBehaviour
{
    public GameObject up;
    public GameObject on;
    public bool isOn;
    public bool isUp;
    // Start is called before the first frame update
    void Start()
    {
        int temp = Random.Range(0,10);
        int temp2 = Random.Range(0,2);
        isOn = (temp>8);
        isUp = (temp2 == 1);
        on.SetActive(isOn);
        up.SetActive(isUp);
        if (isOn)
        {
            Main.Instance.SwitchChange(1);
        }
    }

    public void SwitchActionButton()
    {
        isUp = !isUp;
        isOn = !isOn;
        on.SetActive(isOn);
        up.SetActive(isUp);
        if (isOn)
        {
            Main.Instance.SwitchChange(1);
        }
        else
        {
            Main.Instance.SwitchChange(-1);
        }
    }

    public void ResetTask(){
        int temp = Random.Range(0,10);
        int temp2 = Random.Range(0,2);
        isOn = (temp>8);
        isUp = (temp2 == 1);
        on.SetActive(isOn);
        up.SetActive(isUp);
        if (isOn)
        {
            Main.Instance.SwitchChange(1);
        }
    }

    // Update is called once per frame
}

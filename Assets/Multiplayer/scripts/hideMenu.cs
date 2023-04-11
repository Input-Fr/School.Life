using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideMenu : MonoBehaviour
{
    public GameObject panel;
    public void hideTheMenu()
    {
        panel.SetActive(false);
    }
}

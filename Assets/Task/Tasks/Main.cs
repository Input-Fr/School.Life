using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    static public Main Instance;
    public int switchCount;
    public int onCount = 0;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject key;
    [SerializeField] private PlayerNetwork player;
    [SerializeField] private Transform dropPoint;
    public Switch switch1;
    public Switch switch2;
    public Switch switch3;
    public Switch switch4;
    public Switch switch5;
    public Switch switch6;
    public Switch switch7;
    public Switch switch8;
    public Switch switch9;
    public Switch switch10;
    public Switch switch11;
    public Switch switch12;

    private void Awake()
    {
        Instance = this;
    }


    public void SwitchChange(int points)
    {
        onCount = onCount + points;
        if (onCount == switchCount)
        {
            canvas.SetActive(false);
            player.InstantiateItem(key,dropPoint.position);
            onCount = 0;
            switchCount = 12;
            switch1.ResetTask();
            switch2.ResetTask();
            switch3.ResetTask();
            switch4.ResetTask();
            switch5.ResetTask();
            switch6.ResetTask();
            switch7.ResetTask();
            switch8.ResetTask();
            switch9.ResetTask();
            switch10.ResetTask();
            switch11.ResetTask();
            switch12.ResetTask();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [SerializeField] private Image toFill;

    [HideInInspector] public int duration;
    private int _remainingDuration;

    [HideInInspector] public bool isFinsh;

    private void Start()
    {
        isFinsh = true;
    }

    public void Being(int seconds)
    {
        isFinsh = false;
        duration = seconds;
        _remainingDuration = seconds;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (_remainingDuration >= 0)
        {
            toFill.fillAmount = Mathf.InverseLerp(0, duration, _remainingDuration);
            _remainingDuration--;
            yield return new WaitForSeconds(1f);
        }
        
        isFinsh = true;
    }
}

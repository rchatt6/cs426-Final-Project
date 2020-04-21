using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;

[Serializable]
public class Stats
{
    [SerializeField]
    private BarScript bar;

    [SyncVar]
    public float maxVal;

    [SyncVar]
    public float currentVal;

    public float CurrentVal
    {
        get
        {
            return currentVal;
        }
        set
        {
            this.currentVal = Mathf.Clamp(value,0,MaxVal);
            bar.Value = currentVal;
        }
    }

    public float MaxVal 
    {
        get 
        {
            return maxVal;
        }
        set 
        {
            this.maxVal = value;
            bar.MaxValue = maxVal;
        }
    }

    public void Initialize()
    {
        this.MaxVal = maxVal;
        this.CurrentVal = currentVal;
    }
}
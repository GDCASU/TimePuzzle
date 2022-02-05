using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterTime : MonoBehaviour
{
    public static MasterTime singleton;
    public float timeScale = 1;

    public delegate void UpdateTimeScale(float newTimescale);
    public event UpdateTimeScale updateTimeScaleEvent;
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(gameObject);
    }
    /// <summary>
    /// Changes the value of the timeScale variable and then calls the event so that other scripts can be aware of the change in the global timeScale
    /// </summary>
    /// <returns></returns>
    public void UpdateTime(int value)
    {
        timeScale = value / 10f;
        if (updateTimeScaleEvent != null) updateTimeScaleEvent(timeScale);
    }
}

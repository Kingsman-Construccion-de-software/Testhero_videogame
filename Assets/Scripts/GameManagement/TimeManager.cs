using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public float timeRemaining = 0;
    public bool done = false;
    public bool stopped = true;

    public void SetTimeRemaining(float timeRemaining)
    {
        this.timeRemaining = timeRemaining;
        done = false;
        stopped = false;
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }
    
    public void AddTime(float time)
    {
        if(timeRemaining > 0)
        {
            timeRemaining += time;
        }
    }

    public void Stop()
    {
        stopped = true;
    }

    public bool IsDone()
    {
        return done;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopped)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                done = true;
                stopped = true;
            }
        }
    }
}

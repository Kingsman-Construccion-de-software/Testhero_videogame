using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public float timeRemaining = 0;
    public bool done = false;

    public void SetTimeRemaining(float timeRemaining)
    {
        this.timeRemaining = timeRemaining;
        done = false;
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    public bool IsDone()
    {
        return done;
    }

    

    // Update is called once per frame
    void Update()
    {
        if(timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        } else
        {
            done = true;
        }
    }
}

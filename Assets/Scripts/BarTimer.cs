using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BarTimer : MonoBehaviour
{
    public float timeRemaining = 10;
    public static float timeMax = 10; //same as start of timeRemaining
    public bool timerIsRunning = false;
    public RawImage timeBar;
    //Vector3 scale = new Vector3(0, 0, 0);
    private void Start()
    {
        timerIsRunning = true;
    }
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayBarTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    void DisplayBarTime(float timeToDisplay)
    {
        float length = timeToDisplay / timeMax;
        timeBar.transform.localScale = new Vector3(length, 1, 1);
    }
}
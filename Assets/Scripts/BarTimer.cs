using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BarTimer : MonoBehaviour
{
    public float timeRemaining = 10;
    public static float timeMax = 10; //same as start of timeRemaining
    public bool timerIsRunning = false;
    public RawImage timeBar;
    public bool isStagePlanning = true; // true = planning; false = fighting
    public TMP_Text stageName;
    public GameObject[] DnDScriptGuys;
    public DragAndDrop DnD;

    private void Start()
    {
        timerIsRunning = true;
        DnDScriptGuys = GameObject.FindGameObjectsWithTag("Unit");
    }
    void Update()
    {
        DisplayBarTime();
    }

    void DisplayBarTime()
    {
        //////// Color Stuff ////////

        float length = (timeRemaining / timeMax);
        //Debug.Log("Length: " + length + " \n timeRemaining: " + timeToDisplay + "\n deltaTime" + Time.deltaTime);
        timeBar.transform.localScale = new Vector3(length, 1, 1);
        if (timeRemaining < timeMax / 2.0)
        {
            timeBar.color = new Color((float)(0.9569), (float)(0.8784), (float)(0.3020)); // halfway / warning color 244, 224, 77

            if (timeRemaining < timeMax / 8.0)
            {
                timeBar.color = new Color((float)(0.9020), (float)(0.2039), (float)(0.3843)); // one-eigth / danger color 230, 52, 98
            }
        }
        else
        {
            if (timeRemaining > 0)
                timeBar.color = new Color((float)(0.0392), (float)(0.5059), (float)(0.8196)); // default color 10, 129, 209
        }

        //////// Stage Switching /////////

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            Debug.Log("Time is running: " + timeRemaining);
        }
        else
        {
            Debug.Log("Time has run out! Starting timer over. ");
            timeRemaining = 10;
            if (isStagePlanning)
            {
                isStagePlanning = false;
                //Debug.Log("isStagePlanning = true = " + isStagePlanning);
                stageName.text = "Fighting Stage";
                //DnDScriptGuys.GetComponent<DragAndDrop>().enabled = true;
                foreach (GameObject d in DnDScriptGuys)
                {
                    DnD = d.GetComponent<DragAndDrop>();
                    DnD.isPlanStage = false;
                }


            }
            else
            {
                isStagePlanning = true;
                //Debug.Log("isStagePlanning = false = " + isStagePlanning);
                stageName.text = "Planning Stage";
                //DnDScriptGuys.GetComponent<DragAndDrop>().enabled = false;
                foreach (GameObject d in DnDScriptGuys)
                {
                    DnD = d.GetComponent<DragAndDrop>();
                    DnD.isPlanStage = true;
                }

            }
        }
    }
}
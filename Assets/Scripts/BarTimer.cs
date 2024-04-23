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
    public Material dayM, nightM;
    public ReflectionProbe rp;
    public UnitStore shop;

    private void Start()
    {
        timerIsRunning = true;
        DnDScriptGuys = GameObject.FindGameObjectsWithTag("Unit");
        shop = GameObject.Find("UnitStore").GetComponent<UnitStore>();
    }
    void Update()
    {
        DisplayBarTime();
        DnDScriptGuys = GameObject.FindGameObjectsWithTag("Unit");
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
            //Debug.Log("Time is running: " + timeRemaining);
        }
        else
        {
            //Debug.Log("Time has run out! Starting timer over. ");
            timeRemaining = 10;
            if (isStagePlanning)
            {
                isStagePlanning = false;
                stageName.text = "Fighting Stage";
                
                ////// Change Skybox, Reflections and Fog //////
                RenderSettings.skybox = dayM;
                RenderSettings.fogColor = new Color((float)(0.98), (float)(0.55), (float)(0.32));
                DynamicGI.UpdateEnvironment();
                rp.RenderProbe();

                ////// Stop Units from moving //////
                foreach (GameObject d in DnDScriptGuys)
                {
                    DnD = d.GetComponent<DragAndDrop>();
                    DnD.isPlanStage = false;
                }

            }
            else
            {
                isStagePlanning = true;
                stageName.text = "Planning Stage";

                ////// Fill Store/Bench //////
                shop.FillUnitBench();

                ////// Change Skybox, Reflections and Fog //////
                RenderSettings.skybox = nightM;
                RenderSettings.fogColor = new Color((float)(0.64), (float)(0.41), (float)(0.64));
                DynamicGI.UpdateEnvironment();
                rp.RenderProbe();

                ////// Let Units Move //////
                foreach (GameObject d in DnDScriptGuys)
                {
                    DnD = d.GetComponent<DragAndDrop>();
                    DnD.isPlanStage = true;
                }

            }
        }
    }
}
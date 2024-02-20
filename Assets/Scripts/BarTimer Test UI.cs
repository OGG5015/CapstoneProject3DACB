using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarTimerTestUI : MonoBehaviour
{
    public GameObject timerScreen;
    public bool isOn;
    // Start is called before the first frame update
    void Start()
    {
        isOn = false;
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.T) && (isOn == true))
        {
            print("turn off");
            isOn = false;
            timerScreen.gameObject.SetActive(false);
        } 
       else if (Input.GetKeyDown(KeyCode.T) && (isOn == false))
        {
            print("turn on");
            isOn = true;
            timerScreen.gameObject.SetActive(true);
        }
    }
}

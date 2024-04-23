using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class achbtn : MonoBehaviour
{


    public GameObject achievementList;

    private 

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }



   public void Click()
    {
        achievementList.SetActive(true);
    }
}

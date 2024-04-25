using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class colorchangw : MonoBehaviour
{
    public GameObject myImage;
    public GameObject myImage1;
    public GameObject myImage2;
    public GameObject myImage3;
    public GameObject myImage4;
    public GameObject myImage5;
    //public AI script;

    public void newColor()
    {
        myImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        myImage1.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        myImage2.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        myImage3.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        myImage4.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        myImage5.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }
}

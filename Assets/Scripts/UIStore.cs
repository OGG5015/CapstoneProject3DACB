using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStore : MonoBehaviour
{
    public Sprite portraitR, portraitG, portraitB, portraitGray;
    public int numOfUnits = 3;
    public int cash;
    public TMP_Text w;
    public bool[] isBenchPosFull;

    // Start is called before the first frame update
    void Start()
    {
        isBenchPosFull = new bool[7];
        for (int i = 0; i < isBenchPosFull.Length; i++)
        {
            isBenchPosFull[i] = false;
        }
        FillStore();
        cash = 100;
        UpdateWallet();
    }

    void FillStore()
    {
        foreach (GameObject slot in GameObject.FindGameObjectsWithTag("Shelf")) 
        {
            System.Random rnd = new System.Random();
            int dice = rnd.Next(0, numOfUnits);
            switch (dice)
            {
                case 0:
                    slot.GetComponent<Image>().sprite = portraitR; ; break;
                case 1:
                    slot.GetComponent<Image>().sprite = portraitG; ; break;
                case 2:
                    slot.GetComponent<Image>().sprite = portraitB; ; break;
                default:
                    Debug.Log("numOfUnits exceeds number of avalible units (or some other error)"); break;
            }
        }
    }

    void ClearStore()
    {
        // Change the portraits to blanks
        foreach (GameObject slot in GameObject.FindGameObjectsWithTag("Shelf"))
        {
            slot.GetComponent<Image>().sprite = portraitGray;
        }
    }

    public void ResetStore()
    {
        ClearStore();
        FillStore();
    }

    public void UpdateWallet()
    {
        w.text = "$" + cash;
    }
}

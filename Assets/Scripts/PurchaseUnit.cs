using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using TMPro;
using System;
using System.Drawing;
using System.Linq;
using Unity.Netcode;


public class PurchaseUnit : MonoBehaviour
{
    [field: SerializeField] public UnitBench mindy { get; private set; }
    private HexGrid hexgrid;
    private Vector3 cellCenter;
    public GameObject unitR, unitG, unitB, unitNone;
    public Sprite portraitR, portraitG, portraitB, portraitGray;
    public int numOfUnits = 3;
    public GameObject statBubble;
    public int index;
    public GameObject button;
    public UIStore store;

     private ulong clientOwnerId;

    // Method to set the client owner ID
    public void SetClientOwner(ulong clientId)
    {
        clientOwnerId = clientId;
    }

    public void DisplayStats()
    {
        // determine the unit type
        Sprite portraitX = button.GetComponent<Image>().sprite;
        GameObject unitX = unitNone;
        if (portraitX == portraitR)
        {
            unitX = unitR;
        }
        else if (portraitX == portraitG)
        {
            unitX = unitG;
        }
        else if (portraitX == portraitB)
        {
            unitX = unitB;
        }
        else
        {
            unitX = unitNone;
        }

        // set all the text :[
        statBubble.SetActive(true);
        GameObject temp = GameObject.Find("NameText");
        TMP_Text temp2 = temp.GetComponent<TMP_Text>();
        temp2.text = unitX.name;

        temp = GameObject.Find("HPText");
        temp2 = temp.GetComponent<TMP_Text>();
        temp2.text = unitX.GetComponent<Unit>().maxHp.ToString();

        temp = GameObject.Find("StrengthText");
        temp2 = temp.GetComponent<TMP_Text>();
        temp2.text = unitX.GetComponent<Unit>().str.ToString();

        temp = GameObject.Find("MagicText");
        temp2 = temp.GetComponent<TMP_Text>();
        temp2.text = unitX.GetComponent<Unit>().mag.ToString();

        temp = GameObject.Find("DefenceText");
        temp2 = temp.GetComponent<TMP_Text>();
        temp2.text = unitX.GetComponent<Unit>().def.ToString();

        temp = GameObject.Find("SpiritText");
        temp2 = temp.GetComponent<TMP_Text>();
        temp2.text = unitX.GetComponent<Unit>().spr.ToString();

        temp = GameObject.Find("SpeedText");
        temp2 = temp.GetComponent<TMP_Text>();
        temp2.text = unitX.GetComponent<Unit>().spd.ToString();

        temp = GameObject.Find("RangeText");
        temp2 = temp.GetComponent<TMP_Text>();
        temp2.text = unitX.GetComponent<Unit>().range.ToString();

        temp = GameObject.Find("PriceText");
        temp2 = temp.GetComponent<TMP_Text>();
        temp2.text = unitX.GetComponent<Unit>().price.ToString();

    }

    public void HideStats()
    {
        statBubble.SetActive(false);
    }

    public void ClearShelf()
    {
        button.GetComponent<Image>().sprite = portraitGray;
    }

    public void SummonUnit()
    {
        
        //Debug.Log("SummonUnit is called!");

        if (store.isBenchPosFull.Contains(false))
        {
            // find bench position
            int width = mindy.GetWidth();

            if (mindy != null)
            {
                //Debug.Log("mindy is not null");
                Vector3 benchOrigin = new Vector3(
                    mindy.transform.position.x - (mindy.Width + (mindy.SquareSize / 2)) * mindy.SquareSize / /*2f*/ mindy.Width,
                    mindy.transform.position.y,
                    mindy.transform.position.z
                );
                //Debug.Log("bench origin: " + benchOrigin);

                float distanceFromOriginX = benchOrigin.x;
                int cellIndex = Mathf.FloorToInt(distanceFromOriginX / mindy.SquareSize);
                int ourCellIndex = cellIndex;

                float cellCenterX = benchOrigin.x + (ourCellIndex + 0.5f) * mindy.SquareSize;
                float cellCenterZ = mindy.transform.position.z;
                cellCenter = new Vector3(cellCenterX, mindy.transform.position.y, cellCenterZ);

                // find empty spot on bench to place new unit on
                int index2 = Array.IndexOf(store.isBenchPosFull, false);

                // summon unit based on portrait and subtract money
                Sprite portraitX = button.GetComponent<Image>().sprite;
                GameObject u1;
                Vector3 unitCell;
                if (portraitX == portraitR && store.cash > unitR.GetComponent<Unit>().price)
                {
                    unitCell = new Vector3(cellCenterX + (mindy.SquareSize * index2), mindy.transform.position.y, cellCenterZ);
                    u1 = Instantiate(unitR, unitCell, Quaternion.identity);
                    u1.transform.localScale = new Vector3(5f, 5f, 5f);
                    u1.transform.parent = GameObject.Find("Grid").transform;

                    store.cash = store.cash - unitR.GetComponent<Unit>().price;
                    store.UpdateWallet();
                    store.isBenchPosFull[index2] = true;
                    Debug.Log("Summon Red Guy");

                    
                }
                else if (portraitX == portraitG)
                {
                    unitCell = new Vector3(cellCenterX + (mindy.SquareSize * index2), mindy.transform.position.y, cellCenterZ);
                    u1 = Instantiate(unitG, unitCell, Quaternion.identity);
                    u1.transform.localScale = new Vector3(5f, 5f, 5f);
                    //u1.transform.parent = GameObject.Find("Grid").transform;
                    
                    u1.transform.parent = GameObject.Find("Player1Units").transform;
                    store.cash = store.cash - unitG.GetComponent<Unit>().price;
                    store.UpdateWallet();
                    store.isBenchPosFull[index2] = true;
                    Debug.Log("Summon Green Guy");
                    SetOwnership(u1);
                    //u1.GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
                    Debug.Log("The owner of this unit is " + u1.GetComponent<NetworkObject>().OwnerClientId);
                }
                else if (portraitX == portraitB)
                {
                    unitCell = new Vector3(cellCenterX + (mindy.SquareSize * index2), mindy.transform.position.y, cellCenterZ);
                    u1 = Instantiate(unitB, unitCell, Quaternion.identity);
                    u1.transform.localScale = new Vector3(5f, 5f, 5f);
                    u1.transform.parent = GameObject.Find("Grid").transform;

                    store.cash = store.cash - unitB.GetComponent<Unit>().price;
                    store.UpdateWallet();
                    store.isBenchPosFull[index2] = true;
                    Debug.Log("Summon Blue Guy");
                }
                else
                {
                    Debug.Log("There is no Unit to Summon");
                }
            }
        }
    }
    private void SetOwnership(GameObject unit)
    {
        if (unit.TryGetComponent(out NetworkObject networkObject))
        {
            networkObject.ChangeOwnership(clientOwnerId);

        }
    }

    private ulong GetOwnerID(ulong ownerID)
    {
        ulong clientInstanceId = unitG.GetComponent<NetworkObject>().OwnerClientId;
        return clientInstanceId;
    }
}
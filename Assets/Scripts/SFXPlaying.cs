using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlaying : MonoBehaviour
{
    public AudioSource foley;
    public AudioClip select;
    public AudioClip drop;
    public AudioClip pickup;
    int dice = 0;

    public void PlaySelect()
    {
        foley.clip = select;
        foley.Play();
    }

    public void PlayDrop()
    {
        foley.clip = drop;
        foley.Play();
    }

    public void PlayPickup()
    {
        foley.clip = pickup;
        foley.Play();
    }

    // Test SFX
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            dice = Random.Range(0, 3);
            switch (dice)
            {
                case 0:
                    PlaySelect(); break;
                case 1:
                    PlayDrop(); break;
                case 2:
                    PlayPickup(); break;
            }
        }
    }
}

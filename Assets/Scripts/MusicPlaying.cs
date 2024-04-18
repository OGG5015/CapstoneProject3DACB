using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlaying : MonoBehaviour
{
    public AudioSource radio;
    public AudioClip tests;
    public AudioClip ideas;
    public AudioClip monsters;

    
    public void PlayTests ()
    {
        radio.clip = tests;
    }

public void PlayIdeas()
    {
        radio.clip = ideas;
    }

    public void PlayMonsters()
    {
        radio.clip = monsters;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            print("now playing: Tests by Kevin Perjurer");
            PlayTests();
            radio.Play();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            print("now playing: Ideas by Kevin Perjurer");
            PlayIdeas();
            radio.Play();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            print("now playing: Monsters by Kevin Perjurer");
            PlayMonsters();
            radio.Play();
        }
    }
};

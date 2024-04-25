using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Load Scene
    public void Play()
    {
        SceneManager.LoadScene("Game_Options");

    }

    public float speed;
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * speed);
    }

    //Quit Game
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player has quit the game.");

    }

}

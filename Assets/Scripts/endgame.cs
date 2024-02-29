using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endgame : MonoBehaviour
{
    // Load scene
   public  void  mmenu()
    {
        SceneManager.LoadScene("main_menu");
    }

    // Update is called once per frame
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        print("Button works");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player has quit the game.");
    }
}

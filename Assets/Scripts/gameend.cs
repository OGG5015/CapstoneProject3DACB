using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameend : MonoBehaviour
{
    // Start is called before the first frame update
  public  void Restart()
    {
        SceneManager.LoadScene("bifsudnsidf");
    }

    // Update is called once per frame
   public void Quit()
    {
        Application.Quit();
        Debug.Log("Player has quit the game.");
    }

  public  void Return()
    {

        SceneManager.LoadScene("Main Menu");
    }
}

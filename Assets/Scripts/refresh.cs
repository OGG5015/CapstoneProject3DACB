using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class refresh : MonoBehaviour
{
  public void Refresh()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        print("Button works");
    }

    public void Back()
    {
        SceneManager.LoadScene("Game_Options");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MOPTIONS : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player has quit the game.");

    }

    [SerializeField] Slider vslider;

    void Start()
    {
        if (!PlayerPrefs.HasKey("musicvolume"))
        {
            PlayerPrefs.SetFloat("musicvolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void changeVol()
    {
        AudioListener.volume = vslider.value;
        Save();

    }
    private void Load()
    {
        vslider.value = PlayerPrefs.GetFloat("musicvolume");
    }
    private void Save()
    {
        PlayerPrefs.SetFloat("musicvolume", vslider.value);
    }
}

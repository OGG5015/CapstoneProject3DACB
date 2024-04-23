using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameoptions : MonoBehaviour
{
    // Start is called before the first frame update
   void Start()
    {
        
    }

    public float speed;
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * speed);
    }

    public void splayer()
    {
        SceneManager.LoadScene("Countdown");//Game View
    }

    public void multiplayer()
    {
        
    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    
}

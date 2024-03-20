using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using Unity.Services.Multiplay;
using Unity.Services.Matchmaker.Models;
using TMPro;
using Newtonsoft.Json;
using Unity.Services.Matchmaker;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class ServerStartUp : MonoBehaviour
{
    private string _internalServerIP = "0.0.0.0";
    private string _externalServerIP = "0.0.0.0";
    private ushort _serverPort = 7777;
    //private int 
    async void Start()
    {
        bool server = false;
        var args = System.Environment.GetCommandLineArgs();
        for(int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-dedicatedServer")
            {
                server = true;
            }
            if(args[i] == "-port" && (i + 1 < args.Length))
            {
                _serverPort = (ushort)int.Parse(args[i+1]);
            }
        }
        if (server)
        {
            StartServer();
            await StartServerServices();
        }
    }

    private void StartServer()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(_internalServerIP, _serverPort);
        NetworkManager.Singleton.StartServer();
        
    }

    async Task StartServerServices()
    {
        await UnityServices.InitializeAsync();
        try
        {

        }
        catch(Exception ex)
        {
            
        }
    }

}


/*****************************************************************

FOR MAIN MENU --- CLICKING PLAY BUTTON STARTS MATCHMAKING SERVICE

private void Awake()
{
    playButton.OnClick.AddListener(() => {
        NetworkManager.Singleton.StartHost();
    });
}


*/

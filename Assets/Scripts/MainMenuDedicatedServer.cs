using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Multiplay;
using UnityEngine;
using UnityEngine.SceneManagement;



//this code is to be implemented with the main menu functionalities
public class MainMenuDedicatedServer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if DEDICATED_SERVER
        Debug.Log("DEDICATED_SERVER");
        SceneManager.LoadScene("Game View");
#endif
    }













    /* THIS CODE IS TO ME IMPLEMENTED IN THE LOBBY SCENE 


    private bool alreadyAutoAllocated;
    private static IServerQueryHandler serverQueryHandler;
    private async void InitializeUnityAuthentication()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions initializationOpetions = new InitializationOptions();

            await UnityServices.InitializeAsync(initializationOpetions);
#if !DEDICATED_SERVER
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync();
#endif

#if DEDICATED_SERVER
            Debug.Log("DEDICATED_SERVER LOBBY");

            MultiplayEventCallbacks multiplayEventCallbacks = new MultiplayEventCallbacks();
            multiplayEventCallbacks.Allocate += MultiplayEventCallbacks_Allocate;
            multiplayEventCallbacks.Deallocate += MultiplayEventCallbacks_Deallocate;
            multiplayEventCallbacks.Error += MultiplayEventCallbacks_Error;
            multiplayEventCallbacks.SubscriptionStateChanged += MultiplayEventCallbacks_SubscriptionStateChanged;
            IsServerEvents serverEvents = await MultiplayService.Instance.SubscribeToServerEventsAsync(multiplayEventCallbacks);

            // CHECK THIS LINE
            serverQueryHandler = await MultiplayService.Instance.StartServerQueryHandlerAsync(4, default, "n/a", default, "n/a");

            var serverConfig = MultiplayService.Instance.ServerConfig;
            if (serverConfig.AllocationId != "")
            {
                //Already Allocated
                MultiplayerEventCallbacks_Allocate(new MultiplayAllocation("", serverConfig.ServerId, serverConfig.AllocationId));
            }
    #endif
        }
        //else
        //{
        //    //Already initialized
       // #if DEDICATED_SERVER
       //     Debug.Log("DEDICATED_SERVER LOBBY - ALREADY INIT");
        //}
    }

    private void MultiplayEventCallbacks_Allocate(MultiplayAllocation obj)
    {
        Debug.Log("DEDICATED_SERVER MultiplayEventCallbacks_Allocate");

        if (alreadyAutoAllocated)
        {
            Debug.Log("Already auto allocated!");
            return;
        }

        alreadyAutoAllocated = true;

        var serverConfig = MultiplayService.Instance.ServerConfig;
        Debug.Log($"Server ID: {serverConfig.ServerId}");
        Debug.Log($"Allocation ID: {serverConfig.AllocationId}");
        Debug.Log($"Port: {serverConfig.Port}");
        Debug.Log($"QueryPort: {serverConfig.QueryPort}");
        Debug.Log($"LogDirectory: {serverConfig.ServerLogDirectory}");

        string ipv4Address = "0.0.0.0";
        ushort port = serverConfig.Port;
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipv4Address, port, "0.0.0.0");
        //MultiplayService.Instance.StartServer();
        //StartServer();

        NetworkManager.Singleton.StartServer();
        SceneManager.LoadScene("Game View");
    }

    /*public void StartServer()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        
    }*/

    //private void NetworkManager



}

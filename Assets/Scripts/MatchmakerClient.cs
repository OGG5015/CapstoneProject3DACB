using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Matchmaker;
using Unity.Services.Matchmaker.Models;
using StatusOptions = Unity.Services.Matchmaker.Models.MultiplayAssignment.StatusOptions;
using UnityEngine;
using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;
using Unity.Services.Samples.Friends;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using Unity.Services.Friends;
using Unity.Services.Samples;
using Unity.Services.Lobbies;
using Unity.Services.Multiplay;
using Unity.VisualScripting;
using UnityEngine.Networking;
using Unity.Netcode.Editor.Configuration;
using Unity.Networking.Transport;
using static ServerStartUp;


using UnityEditor.PackageManager;
using Unity.Netcode.Editor;

#if UNITY_EDITOR
using ParrelSync;
#endif

public class MatchmakerClient : MonoBehaviour
{
    private string _ticketId;
    public GameObject PrefabToSpawn;
    public string serviceProfileName;
    public delegate void ServerInfoDelegate(string ipAddress, int port, bool lobbyTicketStatus);
    public static event ServerInfoDelegate OnServerInfoReceived;
    private List<string> playersFromLobby;
    private delegate void TicketStatusDelegate(bool ticketStatus);
    private static event TicketStatusDelegate OnTicketStatusReceived;
    public GameObject startingCanvas;
    public GameObject room1Canvas;
    public GameObject room2Canvas;



    private void OnEnable()
    {
        ServerStartUp.ClientInstance += SignIn;
        ClientLobby.MatchmakerClientInstance += StartClient;
        ClientLobby.OnLobbyInfoReceived += ReceiveLobbyInfo;
    }

    private void OnDisable()
    {
        ServerStartUp.ClientInstance -= SignIn;
    }

    private async void SignIn()
    {
        await ClientSignIn(serviceProfileName);
        await AuthenticationService.Instance.SignInAnonymouslyAsync(); // CHANGE THIS TO USERNAME AND PASSWORD AT A LATER TIME

    }

    private async Task ClientSignIn(string serviceProfileName /* = null*/)
    {

        var initOptions = new InitializationOptions();

        if (serviceProfileName != null)
        {
#if UNITY_EDITOR
            serviceProfileName = $"{serviceProfileName}{GetCloneNumberSuffix()}";
#endif
            Debug.Log($"serviceProfileName: {serviceProfileName}");

            //var initOptions = new InitializationOptions();
            initOptions.SetProfile(serviceProfileName);
            await UnityServices.InitializeAsync(initOptions);

        }
        else
        {

            await UnityServices.InitializeAsync();
        }
        Debug.Log($"Signed In Anonymously as {serviceProfileName}({PlayerID()})");

    }

    private string PlayerID()
    {
        return AuthenticationService.Instance.PlayerId;
    }

#if UNITY_EDITOR
    private string GetCloneNumberSuffix()
    {
        {
            string projectPath = ClonesManager.GetCurrentProjectPath();
            int lastUnderscore = projectPath.LastIndexOf("_");
            string projectCloneSuffix = projectPath.Substring(lastUnderscore + 1);
            if (projectCloneSuffix.Length != 1)
            {
                projectCloneSuffix = "";
            }
            return projectCloneSuffix;
        }
    }
#endif

    public void StartClient()
    {

        //CreateATicket(playersFromLobby);
        CreateATicket();
    }


    public async void CreateATicket()
    {

        //var lobbyPlayers = clientLobbyInstance.GetPlayers();
        var options = new CreateTicketOptions("ACBMultiplayerMode");
        //var players = new List<Player>();

        /*foreach (string player in playersFromLobby)
        {
            players.Add(new Player(player));
        }*/

        var players = new List<Player>
        {
            new Player(
                PlayerID()

                )
        };

        //var lobbyPlayers = clientLobbyInstance.GetPlayers();

        //var players = lobbyPlayers.Select(lobbyPlayer => new Unity.Services.Matchmaker.Models.Player(lobbyPlayer.Id)).ToList();

        //var players = lobbyPlayers;

        Debug.Log($"Players in ticket: \n");
        foreach (var player in players)
        {
            Debug.Log($"Player ID: {player.Id}");
        }

        var ticketResponse = await MatchmakerService.Instance.CreateTicketAsync(players, options);
        _ticketId = ticketResponse.Id;
        Debug.Log($"Ticket ID: {_ticketId}");
        PollTicketStatus();

    }

    public async void CreateATicket(List<string> lobbyPlayers)
    {

        var options = new CreateTicketOptions("ACBMultiplayerMode");
        var players = new List<Player>();

        // Convert each player ID to a Player object and add it to the list of players
        foreach (var playerID in lobbyPlayers)
        {
            players.Add(new Player(playerID));
        }

        Debug.Log("Players in ticket:");
        foreach (var player in players)
        {
            Debug.Log($"Player ID: {player.Id}");
        }

        var ticketResponse = await MatchmakerService.Instance.CreateTicketAsync(players, options);
        _ticketId = ticketResponse.Id;
        Debug.Log($"Ticket ID: {_ticketId}");
        PollTicketStatus();

    }

    private async void PollTicketStatus()
    {
        MultiplayAssignment multiplayAssignment = null;
        bool gotAssignment = false;

        do
        {
            await Task.Delay(TimeSpan.FromSeconds(1.1f));
            var ticketStatus = await MatchmakerService.Instance.GetTicketAsync(_ticketId);
            if (ticketStatus == null) continue;
            if (ticketStatus.Type == typeof(MultiplayAssignment))
            {
                multiplayAssignment = ticketStatus.Value as MultiplayAssignment;

            }
            switch (multiplayAssignment?.Status)
            {

                case StatusOptions.Found:
                    gotAssignment = true;
                    TicketAssigned(multiplayAssignment);
                    //MatchmakerService.Instance.DeleteTicketAsync(multiplayAssignment.ToString());
                    break;

                case StatusOptions.InProgress:
                    Debug.Log($"Waiting...");
                    break;

                case StatusOptions.Failed:
                    gotAssignment = true;
                    Debug.LogError($"Failed to get ticket status.  Error: {multiplayAssignment.Message}");
                    break;

                case StatusOptions.Timeout:
                    gotAssignment = true;
                    Debug.LogError("Failed to get ticket status.  Ticket timed out.");
                    break;

                default:
                    throw new InvalidOperationException();
            }
        } while (!gotAssignment);
    }

    RoomType roomassignment;

    private async void TicketAssigned(MultiplayAssignment assignment)
    {
        //NetworkManager.Singleton.SceneManager.SetClientSynchronizationMode(LoadSceneMode.Single);
        Debug.Log($"Ticket Assigned: {assignment.Ip}:{assignment.Port}");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(assignment.Ip, (ushort)assignment.Port);
        OnServerInfoReceived?.Invoke(assignment.Ip, (int)assignment.Port, true);
        Debug.Log(assignment.MatchId);

        //NetworkManager.Singleton.StartClient();

        //RoomType roomAssignment = GetRoomAssignmentFromServer(); // Implement this method to receive room assignment from server
        //ReactToRoomAssignment(roomAssignment);
        /*foreach (string playerID in playersFromLobby)
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(assignment.Ip, (ushort)assignment.Port, "0.0.0.0");
        }*/
        NetworkManager.Singleton.StartClient();
        await Task.Delay(2000);

        RpcReceiveRoomAssignment(NetworkManager.Singleton.LocalClientId, (int) roomassignment);
        //LoadRoom1();

        //Scene currScene = SceneManager.GetActiveScene();

        //Scene nextScene = SceneManager.GetSceneByName("Game View");

        //string sceneName = NetworkManager.Singleton. <= 1 ? "Game View" : "Game_View";
        //SceneManager.LoadScene("Game View", LoadSceneMode.Single);

        //  IS BEING DESTROYED BECAUSE THIS SCRIPT IS ON GAMEOPTIONS WHICH IS NO LONGER ACTIVE SCENE AFTER THE LOAD SCENE
        //  MAKE SCRIPT NOT DESTROYABLE OR MAKE GAME OBJECT NOT DESTROYABLE.

        //SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Game_Options"));
        // SceneManager.SetActiveScene(nextScene);


        //NetworkManager.Singleton.SceneManager.LoadScene("Game View", LoadSceneMode.Single);
    }

    /*private RoomType GetRoomAssignmentFromServer()
    {
        // Example: Assume server sends room assignment in a message
        return RoomType.Room1; // Replace with logic to receive assignment from server
    }*/

    [ClientRpc]
    public void RpcReceiveRoomAssignment(ulong clientId, int roomAssignmentInt)
    {
        RoomType roomAssignment = (RoomType)roomAssignmentInt;
        Debug.Log($"RoomAssignment {roomAssignment}");

        // React to the received room assignment
        ReactToRoomAssignment(roomAssignment);
    }

    private void ReactToRoomAssignment(RoomType roomAssignment)
    {
        switch (roomAssignment)
        {
            case RoomType.Room1:
                LoadRoom1();
                break;
            case RoomType.Room2:
                LoadRoom2();
                break;
            default:
                Debug.LogError("Invalid room assignment");
                break;
        }
    }

    private void LoadRoom1()
    {
        startingCanvas.SetActive(false);
        room1Canvas.SetActive(true);
        //NetworkManager.Instantiate(PrefabToSpawn, new Vector3(0f, 0f, 0f), Quaternion.identity);
        var prefabInstance = Instantiate(PrefabToSpawn);
        var instanceNetworkObject = prefabInstance.GetComponent<NetworkObject>();
        instanceNetworkObject.Spawn();
    }
    private void LoadRoom2()
    {
        startingCanvas.SetActive(false);
        room2Canvas.SetActive(true);
    }




    /*private void LoadPlayerIcon(int playerNumber)
    {
        if(playerNumber == 1)
        {
            NetworkManager.Singleton.PrefabHandler.AddNetworkPrefab();
        }
        
    }*/

    private void OnTicketAssigned()
    {
        //Debug.Log($"{NetworkManager.Singleton.ConnectedClientsList}");
        //NetworkManager.Singleton.SceneManager.LoadScene("Game View", LoadSceneMode.Additive);
        //SceneManager.LoadScene("Game View");

    }


    private void ReceiveLobbyInfo(List<string> lobbyPlayers)
    {
        playersFromLobby = lobbyPlayers;
        Debug.Log("Lobby players:");
        foreach (string playerID in playersFromLobby)
        {
            Debug.Log(playerID);
        }

    }

    [Serializable]
    public class MatchmakingPlayerData
    {
        public int Skill;
    }
}
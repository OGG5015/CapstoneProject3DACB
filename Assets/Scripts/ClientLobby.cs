using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMPro;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.MemoryProfiler;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

public class ClientLobby : MonoBehaviour
{
    public static ClientLobby Instance { get; private set; }
    private Lobby hostLobby;
    private Lobby joinedLobby;
    private float heartbeatTimer;
    private bool IsPrivate;
    private string playerName;
    private float lobbyUpdateTimer;
    public delegate void LobbyInfoDelegate(List<string> lobbyPlayers);
    public static event LobbyInfoDelegate OnLobbyInfoReceived;

    /*private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        playerName = "Player" + UnityEngine.Random.Range(10, 99);
        Debug.Log(playerName);
    }*/

    private void Awake()
    {
        Instance = this;
        //InitializeUnityAuthentication();
    }

    /*private async void InitializeUnityAuthentication()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions initializationOptions = new InitializationOptions();
            initializationOptions.SetProfile(UnityEngine.Random.Range(0, 10000).ToString());

            await UnityServices.InitializeAsync(initializationOptions);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

    }*/

    private void OnEnable()
    {
        MatchmakerClient.OnServerInfoReceived += ReceiveServerInfo;
        //MatchmakerClient.OnTicketStatusReceived += ReceiveTicketStatus;
    }

    private void OnDisable()
    {
        MatchmakerClient.OnServerInfoReceived -= ReceiveServerInfo;
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
        HandleLobbyPollForUpdates();
    }

    private async void HandleLobbyHeartbeat()
    {
        if (IsLobbyHost())
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f)
            {
                float heartbeatTimerMax = 15;
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }

    private bool IsLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    private async void HandleLobbyPollForUpdates()
    {
        if (joinedLobby != null)
        {
            lobbyUpdateTimer -= Time.deltaTime;
            //Debug.Log("Update timer is NOT null");
            if (lobbyUpdateTimer < 0f)
            {
                float lobbyUpdateTimerMax = 1.1f;
                lobbyUpdateTimer = lobbyUpdateTimerMax;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                joinedLobby = lobby;
            }
        }
        //Debug.Log("Heartbeat timer is null");
    }

    public Text generatedLobbyCodeText;
    public static event System.Action MatchmakerClientInstance;
    public static event System.Action MatchmakerClientLobbyInstance;



    public async void CreateLobby()
    {
        try
        {
            string playerNameLobby = AuthenticationService.Instance.PlayerName;
            string lobbyName = $"{playerNameLobby}Lobby";
            int maxPlayers = 4;

            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
            {
                IsPrivate = false,
                Player = GetPlayer(),
                /*Data = new Dictionary<string, DataObject>{
                    {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, "")}
                }*/
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);

            hostLobby = lobby;
            joinedLobby = hostLobby;




            Debug.Log("Created Lobby! " + lobby.Name + "; Lobby Code: " + lobby.LobbyCode);
            generatedLobbyCodeText.text = lobby.LobbyCode;
            //Update();


        }
        catch (LobbyServiceException e)
        {
            Debug.Log($"There was an issue creating a lobby in ClientLobby CreateLobby(): {e}");
        }
    }

    public void FindMatch()
    {
        OnLobbyInfoReceived?.Invoke(GetLobbyPlayers());
        MatchmakerClientInstance?.Invoke();
        
    }
    public List<string> GetLobbyPlayers()
    {
        List<string> playerIDs = new List<string>();
        foreach (Player player in joinedLobby.Players)
        {
            playerIDs.Add(player.Id);
        }
        return playerIDs;
    }


    private void ReceiveServerInfo(string ipAddress, int port, bool lobbyTicketStatus)
    {
        // Update ClientLobby with the received server information
        if (lobbyTicketStatus == false)
        {
            Debug.Log("Ticket failed.");
        }
        else
        {
            Debug.Log($"Received server info: IP Address - {ipAddress}, Port - {port}");
            
            //ShareNetworkInfoClientRpc(ipAddress, port);
            /*foreach (Player player in joinedLobby.Players)
            {
                if (!IsLobbyHost())
                {
                    
                    var approvalCallback = NetworkManager.Singleton.ConnectionApprovalCallback;
                    Debug.Log(approvalCallback);
                    ShareNetworkInfoClientRpc(ipAddress, port);
                    //NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipAddress, (ushort)port, "0.0.0.0");
                }
            }*/
            
        }
    }

    /*[ClientRpc]
    private void ShareNetworkInfoClientRpc(string ipAddress, int port)
    {
        if(IsLobbyHost()) return;

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipAddress, (ushort) port, "0.0.0.0");
    }*/


    private async void ListLobbies()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            Debug.Log("Lobbies found: " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Data["GameMode"].Value);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log($"There was an issue creating a lobby in ClientLobby CreateLobby(): {e}");
        }

    }

    [SerializeField] private TMP_InputField lobbyCodeInput = default;
    public void JoinByCode()
    {
        string playerLobbyCode = lobbyCodeInput.text;
        JoinLobbyByCode(playerLobbyCode);


    }

    private async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
            {
                Player = GetPlayer()
            };
            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
            joinedLobby = lobby;
            Debug.Log($"Joined lobby with code {lobbyCode}");
            //ClientLobby.Instance.PrintPlayers(joinedLobby);
            //Update();

            foreach (Player player in joinedLobby.Players)
            {
                Debug.Log($"{player.Id}\n");
            }

        }
        catch (LobbyServiceException e)
        {
            Debug.Log($"There was an issue joining the lobby by code in ClientLobby JoinLobbyByCode(): {e}");
        }


    }

    private void PrintPlayers()
    {
        PrintPlayers(joinedLobby);
    }

    private void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Players in Lobby " + lobby.Name + " " + lobby.Data["GameMode"].Value);
        foreach (Player player in lobby.Players)
        {
            Debug.Log(player.Id + " " + player.Data["PlayerName"].Value);
        }
    }


    public List<Player> GetPlayers()
    {
        List<Player> players = joinedLobby.Players;
        return players;
    }

    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>{
                        {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)}
                    }
        };
    }

    private async void UpdateLobbyGameMode(string gameMode)
    {
        try
        {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>{
                {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, gameMode)}
            }
            });
            joinedLobby = hostLobby;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log($"There was an error updating the lobby game mode in private void UpdateLobbyGameMode {e}");
        }

    }

    private async void UpdatePlayerName(string newPlayerName)
    {
        try
        {
            playerName = newPlayerName;
            await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
            {
                Data = new Dictionary<string, PlayerDataObject>{
                        {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)}
                    }
            });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log($"Error updating player name in ClientLobby UpdatePlayerName: {e}");
        }
    }

    private async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);

        }
        catch (LobbyServiceException e)
        {
            Debug.Log($"Error leaving the lobby in ClientLobby LeaveLobby(): {e}");
        }
    }

    private async void KickPlayer()
    {
        try
        {
            // CHANGE THIS --- MUST GRAB ID OF THE PLAYER THAT IS BEING KICKED --- CURRENTLY DOES THE SECOND PLAYER IN THE LOBBY
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, joinedLobby.Players[1].Id);

        }
        catch (LobbyServiceException e)
        {
            Debug.Log($"Error leaving the lobby in ClientLobby LeaveLobby(): {e}");
        }
    }

    private async void MigrateLobbyHost()
    {
        try
        {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                //CHANGE THIS --- CURRENTLY MAKES SECOND PLAYER THE HOST
                HostId = joinedLobby.Players[1].Id
            }
            );
            joinedLobby = hostLobby;
        }
        catch (LobbyServiceException e)
        {
            Debug.Log($"There was an error updating the lobby game mode in private void UpdateLobbyGameMode: {e}");
        }
    }

    private void DeleteLobby()
    {
        try
        {
            LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log($"There was an error deleting the lobby game in ClientLobby DeleteLobby(): {e}");
        }
    }

    /*static async Task<Player> GetPlayerFromAnonymousLoginAsync()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            Debug.Log($"Trying to log in a player ...");

            // Use Unity Authentication to log in
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                throw new InvalidOperationException("Player was not signed in successfully; unable to continue without a logged in player");
            }
        }

        Debug.Log("Player signed in as " + AuthenticationService.Instance.PlayerId);

        // Player objects have Get-only properties, so you need to initialize the data bag here if you want to use it
        return new Player(AuthenticationService.Instance.PlayerId, null, data: new Dictionary<string, PlayerDataObject>());
    }*/
}

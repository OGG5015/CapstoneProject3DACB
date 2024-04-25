using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;
using Unity.Services.Multiplay;
using Unity.Services.Matchmaker.Models;
using Newtonsoft.Json;
using Unity.Services.Matchmaker;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Runtime.CompilerServices;
using Unity.Services.Friends;
//using Unity.Services.Samples.Friends;
//using Matchplay.Server;

public class ServerStartUp : MonoBehaviour
{
    public static event System.Action ClientInstance;
    private const string InternalServerIP = "0.0.0.0";
    private string _externalServerIP = "0.0.0.0";
    private ushort _serverPort = 7777;
    private string _externalConnectionString => $"{_externalServerIP}:{_serverPort}";
    const int _multiplayServiceTimeout = 20000;
    private string _allocationId;
    private BackfillTicket _localBackfillTicket;
    CreateBackfillTicketOptions _createBackfillTicketOptions;
    private const int _ticketChecksMs = 11000;
    private MatchmakingResults _matchmakingPayload;
    private MultiplayEventCallbacks _serverCallbacks;
    private IServerEvents _serverEvents;
    private bool _backfilling = false;
    //MatchmakingResults payload;
    public enum RoomType { Room1, Room2 }


    private IMultiplayService _multiplayService;
    //private RelationshipsManager relationshipsManager;
    //private int 
    async void Start()
    {
        bool server = false;
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-dedicatedServer")
            {
                server = true;
            }

            if (args[i] == "-port" && (i + 1 < args.Length))
            {
                _serverPort = (ushort)int.Parse(args[i + 1]);
            }

            if (args[i] == "-ip" && (i + 1 < args.Length))
            {
                _externalServerIP = args[i + 1];
            }
        }

        if (server)
        {
            //#if dedicatedServer
            Debug.Log("Server detected");
            StartServer();
            await StartServerServices();
            //#endif
        }
        else
        {
            Debug.Log("Client detected");
            ClientInstance?.Invoke();
        }
    }

    private void StartServer()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(InternalServerIP, _serverPort, "0.0.0.0");
        NetworkManager.Singleton.StartServer();
        NetworkManager.Singleton.OnClientDisconnectCallback += ClientDisconnected;
        Debug.Log($"Connected clients: {NetworkManager.Singleton.ConnectedClients.Count}.\nMax players: {ConnectionApprovalHandler.MaxPlayers}");
    }

    async Task StartServerServices()
    {
        await UnityServices.InitializeAsync();
        try
        {
            _multiplayService = MultiplayService.Instance;
            await _multiplayService.StartServerQueryHandlerAsync((ushort)ConnectionApprovalHandler.MaxPlayers, "n/a", "n/a", "0", "n/a");
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Something went wrong trying to set up the SQP service:\n {ex}");
        }

        try
        {
            _matchmakingPayload = await GetMatchmakerPayload(_multiplayServiceTimeout);

            /* this part can be commented out if we don't want this backfilling function -
            Makes it so that a player can join a game even if it's full */
            if (_matchmakingPayload != null)
            {
                Debug.Log($"Got payload: {_matchmakingPayload}");
                await StartBackfill(_matchmakingPayload);
            }
            else
            {
                Debug.LogWarning("Getting the MatchmakerPayload timed out, starting with defaults.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning(message: $"Something went wrong trying to set up the Allocation and Backfill services:\n {ex}");
        }
    }

    private async Task<MatchmakingResults> GetMatchmakerPayload(int timeout)
    {
        var matchmakerPayloadTask = SubscribeAndAwaitMatchmakerAllocation();
        if (await Task.WhenAny(matchmakerPayloadTask, Task.Delay(timeout)) == matchmakerPayloadTask)
        {
            return matchmakerPayloadTask.Result;
        }

        return null;
    }

    private async Task<MatchmakingResults> SubscribeAndAwaitMatchmakerAllocation()
    {
        if (_multiplayService == null) return null;
        _allocationId = null;
        _serverCallbacks = new MultiplayEventCallbacks();
        _serverCallbacks.Allocate += OnMultiplayAllocation;
        _serverEvents = await _multiplayService.SubscribeToServerEventsAsync(_serverCallbacks);

        _allocationId = await AwaitAllocationID();
        var mmPayload = await GetMatchmakerAllocationPayloadAsync();
        return mmPayload;
    }


    private void OnMultiplayAllocation(MultiplayAllocation allocation)
    {
        Debug.Log($"OnAllocation: {allocation.AllocationId}");
        if (string.IsNullOrEmpty(allocation.AllocationId)) return;
        _allocationId = allocation.AllocationId;
    }

    private async Task<string> AwaitAllocationID()
    {
        var config = _multiplayService.ServerConfig;
        Debug.Log($"Awaiting Allocation.  Server config is:\n" +
                    $"-ServerID: {config.ServerId}\n" +
                    $"-AllocationID: {config.AllocationId}\n" +
                    $"-Port: {config.Port}\n" +
                    $"-QueryPort: {config.QueryPort}\n" +
                    $"-logs: {config.ServerLogDirectory}\n");

        while (string.IsNullOrEmpty(_allocationId))
        {
            var configId = config.AllocationId;
            if (string.IsNullOrEmpty(configId) && string.IsNullOrEmpty(_allocationId))
            {
                _allocationId = configId;
                break;
            }

            await Task.Delay(100);
        }

        return _allocationId;
    }

    private async Task<MatchmakingResults> GetMatchmakerAllocationPayloadAsync()
    {
        try
        {
            var payloadAllocation = await MultiplayService.Instance.GetPayloadAllocationFromJsonAs<MatchmakingResults>();
            var modelAsJson = JsonConvert.SerializeObject(payloadAllocation, Formatting.Indented);
            Debug.Log($"{nameof(GetMatchmakerAllocationPayloadAsync)}: \n {modelAsJson}");
            return payloadAllocation;
        }
        catch (Exception ex)
        {
            Debug.LogWarning(message: $"Something went wrong trying to get the MatchmakerPayload in GetMatchmakerAllocationPayloadAsync:\n {ex}");
        }

        return null;
    }

    private async Task StartBackfill(MatchmakingResults payload)
    {
        var backfillProperties = new BackfillTicketProperties(payload.MatchProperties);
        _localBackfillTicket = new BackfillTicket { Id = payload.MatchProperties.BackfillTicketId, Properties = backfillProperties };
        await BeginBackfilling(payload);
    }
    
    private async Task BeginBackfilling(MatchmakingResults payload)
    {
        Debug.Log("Beginning Task BeginBackFilling");
        var matchProperties = payload.MatchProperties;

        if (_backfilling)
        {
            Debug.Log("Already backfilling, no need to start another");
        }

        //MatchplayBackfiller backfiller = new MatchplayBackfiller(_externalConnectionString,"ACBMultiplayerMode",  matchProperties,  10);
        //await backfiller.BeginBackfilling();

        Debug.Log($"Starting backfill Server: {NetworkManager.Singleton.LocalClient}/{4}");
        if (string.IsNullOrEmpty(_localBackfillTicket.Id))
        {

            _createBackfillTicketOptions = new CreateBackfillTicketOptions
            {
                QueueName = payload.QueueName,
                Connection = _externalConnectionString,
                Properties = new BackfillTicketProperties(matchProperties)
            };

            _localBackfillTicket.Id = await MatchmakerService.Instance.CreateBackfillTicketAsync(_createBackfillTicketOptions);
            Debug.Log("_localBackfillTicket.id:" + _localBackfillTicket);

        }
        //SendRoomAssignmentToClient(NetworkManager.Singleton.LocalClientId, GetRoomAssignmentForClient());

        _backfilling = true;
#pragma warning disable 4014
        BackfillLoop();
#pragma warning restore 4014
    }

    private async Task BackfillLoop()
    {
        while (_backfilling /*&& NeedsPlayers()*/)
        {
            _localBackfillTicket = await MatchmakerService.Instance.ApproveBackfillTicketAsync(_localBackfillTicket.Id);
            if (!NeedsPlayers())
            {
                await MatchmakerService.Instance.DeleteBackfillTicketAsync(_localBackfillTicket.Id);
                _localBackfillTicket.Id = null;
                _backfilling = false;

                Debug.Log("private async Task BackfillLoop detecting players no longer needed.");
                return;
            }
            Debug.Log($"Connected clients: {NetworkManager.Singleton.ConnectedClients.Count}.\nMax players: {ConnectionApprovalHandler.MaxPlayers}");
            //SendRoomAssignmentToClient(NetworkManager.Singleton.LocalClientId, GetRoomAssignmentForClient());
            await Task.Delay(_ticketChecksMs);
        }

        _backfilling = false;
    }

    // Helper method to determine room assignment (replace with your logic)
    private RoomType GetRoomAssignmentForClient()
    {
        if (NetworkManager.Singleton.ConnectedClients.Count % 2 == 0)
        {
            return RoomType.Room2;
        }
        else
        {
            return RoomType.Room1;
        }
    }

    private RoomType SendRoomAssignmentToClient(ulong clientId, RoomType roomAssignment)
    {
        int roomAssignmentInt = (int)roomAssignment;

        NetworkManager.Singleton.GetComponent<MatchmakerClient>().RpcReceiveRoomAssignment(clientId, roomAssignmentInt);

        return roomAssignment;
    }

    private async void ClientDisconnected(ulong clientId)
    {

        if (!_backfilling && NetworkManager.Singleton.ConnectedClients.Count > 0 && NeedsPlayers())
        {
            await BeginBackfilling(_matchmakingPayload);
        }
    }

    private bool NeedsPlayers()
    {
        if (NetworkManager.Singleton.ConnectedClients.Count < ConnectionApprovalHandler.MaxPlayers)
        {
            Debug.Log("This game needs players.");
            Debug.Log($"Connected clients: {NetworkManager.Singleton.ConnectedClients.Count}.\nMax players: {ConnectionApprovalHandler.MaxPlayers}");

            return true;
        }
        else
        {
            Debug.Log("This game does not need players.");
            return false;
        }
    }

    private void Dispose()
    {
        _serverCallbacks.Allocate -= OnMultiplayAllocation;
        _serverEvents?.UnsubscribeAsync();
    }

}

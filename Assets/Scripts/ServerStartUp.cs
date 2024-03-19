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

public class ServerStartUp : MonoBehaviour
{
    private string _internalServerIP = "0.0.0.0";
    private string _externalServerIP = "0.0.0.0";
    private ushort _serverPort = 7777;
    //private int 
    /*// Start is called before the first frame update
    void Start()
    {
        bool server = false;
        var args = System.Environment.GetCommandLineArgs();
        for(int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-dedicatedServer")
            {
                server = true;
            }
        }
        if (server)
        {
            StartServer();
        }
    }

    private void StartServer()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>();
        NetworkManager.Singleton.StartServer
        NetworkManager.Singleton.OnClientDisconnectCallback += ClientDisconnected
    }*/

    private IMultiplayService _multiplayService;
    const int _multiplayServiceTimeout = 20000;
    private string _allocationID;
    private MultiplayEventCallbacks _serverCallbacks;
    private IServerEvents _serverEvents;
    private const int _ticketCheckMs = 1000;
    private string _externalConnectingString => $"{_externalServerIP}:{_serverPort}";
    private MatchmakingResults _matchmakingPayload;

    async Task StartServerServices()
    {
        await UnityServices.InitializeAsync();
        try
        {
            _multiplayService = MultiplayService.Instance;
            await _multiplayService.StartServerQueryHandlerAsync(4, "n/a", "n/a", "0", "n/a");
        }
        catch(Exception e)
        {
            Debug.LogWarning($"Something went wrong trying to set up the SQP service:\n{e}");
        }

        try
        {
            var _matchmakingPayload = await GetMatchmakerPayload(_multiplayServiceTimeout);
            if(_matchmakingPayload != null)
            {
                Debug.Log(message: $"Got payload:  {_matchmakingPayload}");
                //await Start
            }
            else
            {
                Debug.LogWarning(message: $"Getting the Matchmaker Payload timed out, starting with defaults");
            }
        }
        catch(Exception e)
        {
            Debug.LogWarning($"Something went wrong trying to set up the Allocation and Backfill services:\n{e}");
        }
    }

    private async Task<MatchmakingResults> GetMatchmakerPayload(int timeout)
    {
        var matchmakerPayloadTask = SubscribeAndAwaitMatchmakerAllocation();
        if(await Task.WhenAny(matchmakerPayloadTask, Task.Delay(timeout)) == matchmakerPayloadTask)
        {
            return matchmakerPayloadTask.Result;
        }

        return null;
    }

    private async Task<MatchmakingResults> SubscribeAndAwaitMatchmakerAllocation()
    {
        if(_multiplayService == null) return null;
        _allocationID = null;
        _serverCallbacks = new MultiplayEventCallbacks();
        _serverCallbacks.Allocate += OnMultiplayAllocation;
        _serverEvents = await _multiplayService.SubscribeToServerEventsAsync(_serverCallbacks);

        _allocationID = await AwaitAllocationID();
        var mmPayload = await GetMatchmakerPayloadAllocationAsync();
        return mmPayload;
    }

    

    private void OnMultiplayAllocation(MultiplayAllocation allocation)
    {
        Debug.Log($"OnAllocation: {allocation.AllocationId}");
        if(string.IsNullOrEmpty(allocation.AllocationId)) return;
        _allocationID = allocation.AllocationId;
    }

    private async Task<string> AwaitAllocationID()
    {
        var config = _multiplayService.ServerConfig;
        Debug.Log("Awaiting Allocation. Server Config is:\n" +
            $"-ServerID: {config.ServerId}\n" +
            $"-AllocationID: {config.AllocationId}\n" +
            $"-Port: {config.Port}\n" +
            $"-QPort: {config.QueryPort}\n" +
            $"-logs: {config.ServerLogDirectory}");
        
        while(string.IsNullOrEmpty(_allocationID))
        {
            var configId = config.AllocationId;
            if(!string.IsNullOrEmpty(configId) && string.IsNullOrEmpty(_allocationID))
            {
                _allocationID = configId;
                break;
            }
            await Task.Delay(100);
        }

        return _allocationID;
    }

    private async Task<MatchmakingResults> GetMatchmakerPayloadAllocationAsync()
    {
        try
        {
            var payloadAllocation = await MultiplayService.Instance.GetPayloadAllocationFromJsonAs<MatchmakingResults>();
            var modelAsJson = JsonConvert.SerializeObject(payloadAllocation, Formatting.Indented);
            Debug.Log($"{nameof(GetMatchmakerPayloadAllocationAsync)}:\n{modelAsJson}");
            return payloadAllocation;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Something went wrong trying to get the Matchmaker Parload in GetMatchmakerPayloadAllocationAsync:\n{e}");
        }

        return null;

    }

    private bool NeedsPlayers()
    {
        //do something
        return false;
    }

    private void Dispose()
    {
        _serverCallbacks.Allocate -= OnMultiplayAllocation;
        _serverEvents?.UnsubscribeAsync();
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

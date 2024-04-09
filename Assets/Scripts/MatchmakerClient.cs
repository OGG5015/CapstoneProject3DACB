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


//using Unity.Netcode.Editor;


#if UNITY_EDITOR
using ParrelSync;
#endif

public class MatchmakerClient : MonoBehaviour
{
    private string _ticketId;
    public GameObject PrefabToSpawn;
    public string serviceProfileName;

    private void OnEnable()
    {
        ServerStartUp.ClientInstance += SignIn;
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

    private async Task ClientSignIn(string serviceProfileName/* = null*/)
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

        CreateATicket();
    }

    private async void CreateATicket()
    {
        var options = new CreateTicketOptions("ACBMultiplayerMode");

        var players = new List<Player>
        {
            new Player(
                PlayerID()/*,
                new MatchmakingPlayerData
                {
                    Skill = 100
                }*/

                )
        };

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

    private void TicketAssigned(MultiplayAssignment assignment)
    {
        Debug.Log($"Ticket Assigned: {assignment.Ip}:{assignment.Port}");
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(assignment.Ip, (ushort)assignment.Port);
        NetworkManager.Singleton.StartClient();

        //UnityEngine.SceneManagement.SceneManager.LoadScene("Game View");

       // OnTicketAssigned();
       

       



       //SceneManager.LoadScene("Game View", LoadSceneMode.Additive);
        //SceneManager.LoadSceneAsync("Game View");
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game View"));
        //SceneManager.UnloadSceneAsync("Game_Options");
        //LoadPlayerIcon(playerNumber);
        SceneManager.LoadSceneAsync("Game View");

        //Scene nextscene = SceneManager.GetSceneByName("Game View");
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game View"));
        //SceneManager.UnloadSceneAsync("Game_Options");
       

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

    [Serializable]
    public class MatchmakingPlayerData
    {
        public int Skill;
    }

}

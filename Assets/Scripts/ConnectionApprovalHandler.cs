using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ConnectionApprovalHandler : MonoBehaviour
{
    public static int MaxPlayers  = 10;

    public void Awake()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
    }

    public void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        Debug.Log("Connection Approval");
        response.Approved = true;
        response.CreatePlayerObject = true;
        response.PlayerPrefabHash = null;
        if(NetworkManager.Singleton.ConnectedClients.Count >= MaxPlayers)
        {
            response.Approved = false;
            response.Reason = "Server is full";
        }
        response.Pending = false;
        
    }
}


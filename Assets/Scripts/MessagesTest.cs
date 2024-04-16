using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Unity.Networking;
using Unity.Netcode;
using System;
using Unity.Collections;
using UnityEditor.Il2Cpp;
using Unity.VisualScripting;



public class MessagesTest : NetworkBehaviour
{
    [Tooltip("The name identifier used for this custom message handler.")]
    public string MessageName = "MyCustomNamedMessage";

    /// <summary>
    /// For most cases, you want to register once your NetworkBehaviour's
    /// NetworkObject (typically in-scene placed) is spawned.
    /// </summary>
    public override void OnNetworkSpawn()
    {
        // Both the server-host and client(s) register the custom named message.
        NetworkManager.CustomMessagingManager.RegisterNamedMessageHandler(MessageName, ReceiveMessage);

        if (IsServer)
        {
            // Server broadcasts to all clients when a new client connects (just for example purposes)
            NetworkManager.OnClientConnectedCallback += OnClientConnectedCallback;
        }
        else
        {
            // Clients send a unique Guid to the server
            SendMessage(Guid.NewGuid());
        }
    }

    private void OnClientConnectedCallback(ulong obj)
    {
        SendMessage(Guid.NewGuid());
    }

    public override void OnNetworkDespawn()
    {
        // De-register when the associated NetworkObject is despawned.
        NetworkManager.CustomMessagingManager.UnregisterNamedMessageHandler(MessageName);
        // Whether server or not, unregister this.
        NetworkManager.OnClientDisconnectCallback -= OnClientConnectedCallback;
    }

    /// <summary>
    /// Invoked when a custom message of type <see cref="MessageName"/>
    /// </summary>
    private void ReceiveMessage(ulong senderId, FastBufferReader messagePayload)
    {
        var receivedMessageContent = new ForceNetworkSerializeByMemcpy<Guid>(new Guid());
        messagePayload.ReadValueSafe(out receivedMessageContent);
        if (IsServer)
        {
            Debug.Log($"Server received GUID ({receivedMessageContent.Value}) from client ({senderId})");
        }
        else
        {
            Debug.Log($"Client received GUID ({receivedMessageContent.Value}) from the server.");
        }
    }

    /// <summary>
    /// Invoke this with a Guid by a client or server-host to send a
    /// custom named message.
    /// </summary>
    public void SendMessage(Guid inGameIdentifier)
    {
        var messageContent = new ForceNetworkSerializeByMemcpy<Guid>(inGameIdentifier);
        var writer = new FastBufferWriter(1100, Allocator.Temp);
        var customMessagingManager = NetworkManager.CustomMessagingManager;
        using (writer)
        {
            writer.WriteValueSafe(messageContent);
            if (IsServer)
            {
                // This is a server-only method that will broadcast the named message.
                // Caution: Invoking this method on a client will throw an exception!
                customMessagingManager.SendNamedMessageToAll(MessageName, writer);
            }
            else
            {
                // This is a client or server method that sends a named message to one target destination
                // (client to server or server to client)
                
                customMessagingManager.SendNamedMessage(MessageName, NetworkManager.ServerClientId, writer);
            }
        }
        
    }
}




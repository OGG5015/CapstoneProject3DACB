using System;
using System.Data;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerUnitManager : NetworkBehaviour
{
    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<Vector3> unitPosition = new NetworkVariable<Vector3>();
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            //SetClientOwnership();
        }
        //Initialize();
        
        //unitPosition.Settings.WritePermission = NetworkVariablePermission.OwnerOnly;
        //unitPosition.Settings.ReadPermission = NetworkVariablePermission.Everyone;

        // Register callback for position changes
        unitPosition.OnValueChanged += OnUnitPositionChanged;
    }
    

    /*private void Update()
    {
        //Debug.Log(OwnerClientId + "; randomNumber" + randomNumber.Value);
        if(!IsOwner) return;
        
        if(NetworkObject.GetComponent<DragAndDrop>().isDragging)
        {
            unitPosition.Value = NetworkObject.GetComponent<DragAndDrop>().transform.position;
        }
        unitPosition.Value = NetworkObject.GetComponent<Vector3>();
    }*/

    private void OnDestroy()
    {
        if (IsOwner)
        {
            CustomSceneManager.Instance.PlayerLeft();
        }
    }

    /*private void SetClientOwnership()
    {
        GameObject playerUnit = GameObject.FindGameObjectWithTag("Unit");

        if (playerUnit.TryGetComponent(out NetworkObject networkObject))
            {
                // Ensure client ownership for the unit
                networkObject.ChangeOwnership(NetworkManager.Singleton.LocalClientId);
            }
        
    }*/

    private void SetClientOwnership()
    {
        GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("Unit");

        foreach (var unit in playerUnits)
        {
            if (unit.TryGetComponent(out NetworkObject networkObject))
            {
                networkObject.ChangeOwnership(NetworkManager.Singleton.LocalClientId);
            }
        }
    }

    private void OnUnitPositionChanged(Vector3 oldValue, Vector3 newValue)
    {
        if (!IsOwner)
        {
            // Update the position of the unit for non-owner clients
            transform.position = newValue;

            Vector3 localUnitScale = new Vector3(4, 46, 4);
            transform.localScale = localUnitScale;
        }
    }
}



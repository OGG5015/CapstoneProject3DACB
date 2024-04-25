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
            SetClientOwnership();
        }
        //Initialize();
    }
    

    private void Update()
    {
        //Debug.Log(OwnerClientId + "; randomNumber" + randomNumber.Value);
        if(!IsOwner) return;
        
        if(NetworkObject.GetComponent<DragAndDrop>().isDragging)
        {
            unitPosition.Value = NetworkObject.GetComponent<DragAndDrop>().transform.position;
        }
        unitPosition.Value = NetworkObject.GetComponent<Vector3>();
    }

    private void OnDestroy()
    {
        if (IsOwner)
        {
            CustomSceneManager.Instance.PlayerLeft();
        }
    }

    private void SetClientOwnership()
    {
        GameObject playerUnit = GameObject.FindGameObjectWithTag("Unit");

        if (playerUnit.TryGetComponent(out NetworkObject networkObject))
            {
                // Ensure client ownership for the unit
                networkObject.ChangeOwnership(NetworkManager.Singleton.LocalClientId);
            }
        
    }
}



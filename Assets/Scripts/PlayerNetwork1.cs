using System;
using System.Data;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerNetwork : NetworkBehaviour
{
    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<Vector3> unitPosition = new NetworkVariable<Vector3>();

    private void Start()
    {
        if(IsOwner)
        {
            CustomSceneManager.Instance.PlayerJoined();
        }
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            SetClientOwnership();
        }
        //Initialize();
    }
    /*public override void OnNetworkSpawn()
    {
        //base.OnNetworkSpawn();
        //Initialize();
        
        if (IsServer)
        {
            // Initialize the position of the unit
            unitPosition.Value = transform.position;
        }
        // Register callback to update unit position when it changes
        unitPosition.OnValueChanged += OnUnitPositionChanged;
    }

    private void OnUnitPositionChanged(Vector3 oldValue, Vector3 newValue)
    {
        if (!IsServer)
        {
            // Update the position of the unit for non-server clients
            transform.position = newValue;
        }
    }

    private void OnDestroy()
    {
        unitPosition.OnValueChanged -= OnUnitPositionChanged;
    }*/

    private void Update()
    {
        //Debug.Log(OwnerClientId + "; randomNumber" + randomNumber.Value);
        if(!IsOwner) return;
        
        if(NetworkObject.GetComponent<DragAndDrop>().isDragging)
        {
            unitPosition.Value = NetworkObject.GetComponent<Vector3>();
        }
        Vector3 moveDir = new Vector3(0,0,0);

        if(Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if(Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if(Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if(Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveDir *moveSpeed*Time.deltaTime;
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



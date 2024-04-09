using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerNetwork : NetworkBehaviour
{
    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    /*private Camera _mainCamera;

    private void Initialize()
    {
        _mainCamera = Camera.main;

    }*/

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        //Initialize();
    }

    private void Update()
    {
        //Debug.Log(OwnerClientId + "; randomNumber" + randomNumber.Value);
        if(!IsOwner) return;
        
        if(Input.GetKeyDown(KeyCode.T))
        {
            randomNumber.Value = Random.Range(0,100);
        }
        Vector3 moveDir = new Vector3(0,0,0);

        if(Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        if(Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if(Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if(Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveDir *moveSpeed*Time.deltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerConnection : NetworkBehaviour
{
    public GameObject PlayerUnitPrefab1;
    public GameObject PlayerUnitPrefab2;

    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        CmdSpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
    }

    [Command]
    void CmdSpawnPlayer()
    {
        Debug.Log("Connections" + NetworkServer.connections.Count);

        if (NetworkServer.connections.Count % 2 == 0)
        {
            //GameObject go = Instantiate(PlayerUnitPrefab1);
            //NetworkServer.Spawn(go, connectionToClient);
        }
        else
        {
            //GameObject go = Instantiate(PlayerUnitPrefab2);
            //NetworkServer.Spawn(go, connectionToClient);
        }
    }
}

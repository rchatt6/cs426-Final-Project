using UnityEngine;
using System.Collections;
using Mirror;

public class SpawnManager_BridgePiecesSpawner : NetworkBehaviour {


	[SerializeField] GameObject bridgePiecesPrefab;
	private GameObject[] bridgeSpawns;
	private int counter;
	
	[SerializeField]
	private float waveRate = 5;
	
	[SerializeField]
    private int maxNumberOfBridgePieces = 20;
	
	[SerializeField]
    private int numberOfBridgePieces = 20;
	
	private bool isSpawnActivated = true;
	
	public override void OnStartServer ()
	{
		bridgeSpawns = GameObject.FindGameObjectsWithTag("bridgePiecesSpawn");
		StartCoroutine(bridgePiecesSpawner());
	}
	
	
	/*[ClientRpc]
	public override void RpcOnStartClient()
	{
		
	}*/
	
	IEnumerator bridgePiecesSpawner()
	{
		for(;;)
		{
			yield return new WaitForSeconds(waveRate);
			GameObject[] bridge = GameObject.FindGameObjectsWithTag("bridge");
			if(bridge.Length < maxNumberOfBridgePieces)
			{
				CommenceSpawn();
			}
		}
	}
	
	void CommenceSpawn()
	{
		if(isSpawnActivated)
		{
			for(int i = 0; i < numberOfBridgePieces; i++)
			{
				//int randomIndex = Random.Range(0, bridgeSpawns.Length);
				RpcSpawnBridgePieces(bridgeSpawns[i].transform.position);
			}
		}
	}
	
	/*[Command]
	void CmdSpawnBridgePieces(GameObject gg)
	{
		ClientScene.RegisterPrefab(gg);
		RpcSpawnBridgePieces(gg);
	}
	
	[ClientRpc]
	void RpcSpawnBridgePieces(GameObject gg)
	{
		NetworkServer.Spawn(gg);
	}*/
	
	private void FixedUpdate()
    {
		//RpcSpawnBridgePieces(
	}
	
	/*[ClientRpc]
	void RpcUpdateSpawnBridgePiecesPos(Vector3 spawnPos)
	{
		
	}*/
	
	[ClientRpc]
	void RpcSpawnBridgePieces(Vector3 spawnPos)
	{
		counter++;
		GameObject go = GameObject.Instantiate(bridgePiecesPrefab, spawnPos, Quaternion.identity) as GameObject;
		go.GetComponent<BridgeID>().bridgeID = "Bridge Piece " + counter;
		/*if(!isServer){
			ClientScene.RegisterPrefab(bridgePiecesPrefab);
		}*/
		NetworkServer.Spawn(go);
		//NetworkServer.FindLocalObject(go.GetComponent<NetworkIdentity>().netid);
		//CmdSpawnBridgePieces(go);
	}
}
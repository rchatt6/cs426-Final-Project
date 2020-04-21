using UnityEngine;
using System.Collections;
using Mirror;

public class SpawnManager_ZombieSpawner : NetworkBehaviour {

	[SerializeField] GameObject zombiePrefab;
	private GameObject[] zombieSpawns;
	private int counter;
    [SerializeField]
    private int numberOfZombies = 20;
    [SerializeField]
    private int maxNumberOfZombies = 400;
    [SerializeField]
    private float waveRate = 5;
	private bool isSpawnActivated = true;

	public override void OnStartServer ()
	{
		zombieSpawns = GameObject.FindGameObjectsWithTag("ZombieSpawn");
		StartCoroutine(ZombieSpawner());
	}

	IEnumerator ZombieSpawner()
	{
		for(;;)
		{
			yield return new WaitForSeconds(waveRate);
			GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
			if(zombies.Length < maxNumberOfZombies)
			{
				CommenceSpawn();
			}
		}
	}

	void CommenceSpawn()
	{
		if(isSpawnActivated)
		{
			for(int i = 0; i < numberOfZombies; i++)
			{
				int randomIndex = Random.Range(0, zombieSpawns.Length);
				SpawnZombies(zombieSpawns[randomIndex].transform.position);
			}
		}
	}

	void SpawnZombies(Vector3 spawnPos)
	{
		counter++;
		GameObject go = GameObject.Instantiate(zombiePrefab, spawnPos, Quaternion.identity) as GameObject;
		go.GetComponent<ZombieID>().zombieID = "Zombie " + counter;
		NetworkServer.Spawn(go);
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ZombieSpawner : NetworkBehaviour
{
	
	[SerializeField] GameObject zombiePrefab;
	[SerializeField] GameObject zombieSpawn;
	
	private int counter;
	private int numZombies = 50;
	
	public override void OnStartServer(){
		for(int i=0;i<numZombies;i++){
			SpawnZombies();
		}
	}
	
	void SpawnZombies(){
		counter++;
		
		GameObject go = GameObject.Instantiate(zombiePrefab, zombieSpawn.transform.position, Quaternion.identity) as GameObject;
		NetworkServer.Spawn(go);
		go.GetComponent<ZombieID>().zombieID = "Zombie " + counter;
		
	}
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

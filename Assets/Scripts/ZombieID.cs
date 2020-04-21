using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ZombieID : NetworkBehaviour
{
	[SyncVar] public string zombieID;
	private Transform myTransform;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        SetIdentity();
    }
	
	void SetIdentity(){
		if(myTransform.name == "" || myTransform.name == "Zombie(Clone)"){
			myTransform.name = zombieID;
		}
	}
}

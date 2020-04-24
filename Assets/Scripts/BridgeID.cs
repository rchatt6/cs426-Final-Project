using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BridgeID : NetworkBehaviour
{
	[SyncVar] public string bridgeID;
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
		if(myTransform.name == "" || myTransform.name == "Bridge Piece(Clone)"){
			myTransform.name = bridgeID;
		}
	}
}

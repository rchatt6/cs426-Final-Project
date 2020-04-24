using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Pickup3 : NetworkBehaviour
{
    //[SyncVar]
	float throwForce = 600;
	//[SyncVar]
	Vector3 objectPos;
	//[SyncVar]
	float distance;
	
	/*[SyncVar(hook = "hookOnHeldChanged")]
	NetworkIdentity m_heldObject;*/
	
	//[SyncVar]
	public bool canhold = true;
	//[SyncVar]
	public GameObject item;
	//[SyncVar]
	private GameObject tempParent;
	//[SyncVar]
	public bool isHolding = false;
	
	//NetworkIdentity id = item.GetComponent<NetworkIdentity>();
	
	
	/*[SerializeField]
	Transform heldParent;
	void hookOnHeldChanged(NetworkIdentity id)
	{
		GameObject heldObj = ClientScene.FindLocalObject(id);
		heldObj.transform.parent = heldParent;
	}*/
	
	/*[Server]
	public override void OnStartClient()
	{
		this.transform.position = 
	}*/
	
	/*void FixedUpdate ()
    {
		if(!isLocalPlayer){
			//CmdSendPos(transform.localPosition, transform.localRotation, playerBody.transform.localRotation, physicsRoot.velocity, transform.parent.name);
		}
		//CmdSendPos(this.transform.position);
		//Debug.Log(item.GetComponent<N);
	}*/
	
	void Update ()
	{
        //Debug.Log("isClient: " + isClient);

        /*if (!isClient)
            return;*/
		
		/*if (!isServer)
        {
            CmdSendPos(objectPos);
        }*/

        tempParent = GameObject.FindWithTag("Destination");
        //Debug.Log("tempParent: " + tempParent);

        if (tempParent)
        {
            distance = Vector3.Distance(item.transform.position, tempParent.transform.position);
            if (distance >= 4f)
            {
                isHolding = false;
            }
            //Check of isHolding
            if (isHolding == true)
            {
                item.GetComponent<Rigidbody>().velocity = Vector3.zero;
                item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                item.transform.SetParent(tempParent.transform);


                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    //throw
                    item.GetComponent<Rigidbody>().AddForce(tempParent.transform.forward * throwForce);
                    isHolding = false;
                }
            }

            else
            {
                objectPos = item.transform.position;
                item.transform.SetParent(null);
                item.GetComponent<Rigidbody>().useGravity = true;
                item.transform.position = objectPos;
            }
        }
		
		
		if (Input.GetKeyDown(KeyCode.Mouse2))
        {
			/*if (!isClient)
				return;*/
			
			/*if (!isServer)
			{
				CmdSendPos(objectPos);
			}*/

			if (distance <= 4f)
			{
				isHolding = true;
				item.GetComponent<Rigidbody>().useGravity = false;
				item.GetComponent<Rigidbody>().detectCollisions = true;
				//item.GetComponent<Rigidbody>().freezeRotation = false;
				//item.GetComponent<Rigidbody>().isKinematic = true;
			}
		}
		
		if (Input.GetKeyUp(KeyCode.Mouse2))
        {
			/*if (!isClient)
				return;*/
			
			/*if (!isServer)
			{
				CmdSendPos(objectPos);
			}*/

			isHolding = false;
			item.GetComponent<Rigidbody>().useGravity = true;
			//item.GetComponent<Rigidbody>().freezeRotation = true;
		}
	}
	
	[Command]
	void CmdSendPos(Vector3 objPos){
		RpcUpdatePos(objPos);
	}
	
	[ClientRpc]
	void RpcUpdatePos(Vector3 objPos){
		this.objectPos = objPos;
	}
	
	/*void OnMouseDown()
	{
        if (!isClient)
            return;

        if (distance <= 4f)
		{
			isHolding = true;
			item.GetComponent<Rigidbody>().useGravity = false;
			item.GetComponent<Rigidbody>().detectCollisions = true;
            //item.GetComponent<Rigidbody>().freezeRotation = false;
            //item.GetComponent<Rigidbody>().isKinematic = true;
        }
	}*/
	
	/*void OnMouseUp()
	{
        if (!isClient)
            return;

        isHolding = false;
        item.GetComponent<Rigidbody>().useGravity = true;
        //item.GetComponent<Rigidbody>().freezeRotation = true;
    }*/
}
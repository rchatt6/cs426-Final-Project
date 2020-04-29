using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Pickup2 : NetworkBehaviour
{
    //[SyncVar]
	public float throwForce = 400;
	//[SyncVar]
	Vector3 objectPos;
	//[SyncVar]
	float distance;
	
	//[SyncVar]
	public bool canhold = true;
	//[SyncVar]
	public GameObject item;
	//[SyncVar]
	private GameObject tempParent;
	//[SyncVar]
	public bool isHolding = false;

    void Start()
    {
        if (!isServer)
        {
            return;
        }


        item.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    void Update ()
	{
        //Debug.Log("isClient: " + isClient);

        /*if (!isClient)
            return;*/
		
		if (!isServer)
        {
            CmdSendPos(objectPos);
        }

        tempParent = GameObject.FindWithTag("Destination");
        //Debug.Log("tempParent: " + tempParent);

        if (tempParent)
        {
            distance = Vector3.Distance(item.transform.position, tempParent.transform.position);
            if (distance >= 4f)
            {
                isHolding = false;
                item.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
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

                    //make bridge piece semi-transparent
                    item.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                }
            }

            else
            {
                objectPos = item.transform.position;
                item.transform.SetParent(null);
                item.GetComponent<Rigidbody>().useGravity = true;
                item.GetComponent<BoxCollider>().enabled = true;
                item.transform.position = objectPos;
            }
        }
		
		
		if (Input.GetKeyDown(KeyCode.Mouse2))
        {
			/*if (!isClient)
				return;*/
			
			if (!isServer)
			{
				CmdSendPos(objectPos);
			}

			if (distance <= 4f)
			{
				isHolding = true;
				item.GetComponent<Rigidbody>().useGravity = false;
				item.GetComponent<Rigidbody>().detectCollisions = true;
                item.GetComponent<BoxCollider>().enabled = false;

                item.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

                //item.GetComponent<Rigidbody>().freezeRotation = false;
                //item.GetComponent<Rigidbody>().isKinematic = true;
            }
		}
		
		if (Input.GetKeyUp(KeyCode.Mouse2))
        {
			/*if (!isClient)
				return;*/
			
			if (!isServer)
			{
				CmdSendPos(objectPos);
			}

			isHolding = false;
			item.GetComponent<Rigidbody>().useGravity = true;
            item.GetComponent<BoxCollider>().enabled = true;

            item.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            //item.GetComponent<Rigidbody>().freezeRotation = true;
        }
	}
	
	[Command]
	void CmdSendPos(Vector3 objPos){
		RpcUpdatePos(objPos);
	}
	
	[ClientRpc]
	void RpcUpdatePos(Vector3 objPos){
		objectPos = objPos;
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